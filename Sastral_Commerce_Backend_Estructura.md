# Estructura de backend (Clean Architecture + Capas)

Este documento define una estructura de carpetas para los servicios backend de Sastral Commerce (por bounded context) y provee clases base mínimas para iniciar.

## Objetivo

- Separar responsabilidades por capas: Dominio, Aplicación, Infraestructura, API (y Workers opcional).
- Reutilizar patrones consistentes en cada servicio (`Sastral.Tienda`, `Sastral.Gestion`).
- Ubicar DTOs en `Application/Features/<Feature>/DTOs`.

## Árbol de carpetas (por servicio)

```text
/ecom-backend
  /shared/BuildingBlocks
    README.md
  /Sastral.Tienda
    /src
      Sastral.Tienda.Api/
        Program.cs
        /Controllers/
        /Contracts/          (opc. request/response públicos)
        /Extensions/         (Swagger, ProblemDetails, Auth)
      Sastral.Tienda.Application/
        /Common/             (Result, PagedResult)
        /Abstractions/       (IRepository, IUnitOfWork, IDateTime)
        /Features/
          /Products/
            /DTOs/           (CreateProductDto, UpdateProductDto, ProductResponseDto)
            /Validation/     (FluentValidation)
            /Mappings/       (AutoMapper Profile)
            /Queries/        (GetById, ListPaged)
            /Commands/       (Create, Update, Delete)
      Sastral.Tienda.Domain/
        /Common/             (BaseEntity, BaseAuditableEntity, ValueObject)
        /Products/           (Product aggregate, domain events opc.)
      Sastral.Tienda.Infrastructure/
        /Persistence/        (AppDbContext, Configurations, Migrations)
        /Repositories/       (EF repos + UoW)
        /Messaging/          (RabbitMQ publisher/consumer opc.)
        /External/           (MercadoPago/MercadoLibre clients)
        DependencyInjection.cs
    /tests
      Sastral.Tienda.Domain.Tests/
      Sastral.Tienda.Application.Tests/
      Sastral.Tienda.Api.IntegrationTests/
  /Sastral.Gestion
    (misma estructura que Sastral.Tienda)
```

## Descripción por capa y carpeta

- **Dominio** (`Sastral.<Servicio>.Domain`)
  - `Common/`: abstracciones transversales de dominio (`BaseEntity`, `BaseAuditableEntity`, `ValueObject`).
  - `<Aggregate>/`: entidades/agregados y reglas invariantes del negocio. Sin dependencias a infraestructura.
- **Aplicación** (`Sastral.<Servicio>.Application`)
  - `Common/`: utilitarios de aplicación (`Result`, `PagedResult`).
  - `Abstractions/`: contratos hacia afuera (repositorios, UoW, tiempo, mensajería).
  - `Features/<Feature>/`: casos de uso por funcionalidad (DTOs, Validations, Mappings, Queries/Commands).
- **Infraestructura** (`Sastral.<Servicio>.Infrastructure`)
  - `Persistence/`: `DbContext`, configuraciones EF Core (`IEntityTypeConfiguration<>`), migraciones.
  - `Repositories/`: implementación de `IRepository<>`/UoW.
  - `Messaging/`, `External/`: integraciones técnicas (RabbitMQ, Mercado Pago/Libre).
  - `DependencyInjection.cs`: método `AddInfrastructure(...)` para registrar servicios.
- **API** (`Sastral.<Servicio>.Api`)
  - `Program.cs`: bootstrap minimal APIs/Controllers, Swagger, ProblemDetails, JWT.
  - `Controllers/`: controladores finos que orquestan casos de uso.
  - `Contracts/` (opcional): contratos públicos de request/response si se desean desacoplar de Application DTOs.
  - `Extensions/`: extensiones para configurar Swagger, Auth, ProblemDetails.
- **Tests** (`/tests`)
  - `Domain.Tests`, `Application.Tests`, `Api.IntegrationTests` (TestServer).

## Ubicación de DTOs

- Por defecto en `Application/Features/<Feature>/DTOs` (para orquestación interna y mapeos).
- Alternativa (si se desea aislar API): `Api/Contracts` para requests/responses públicos, y mapas entre Contracts y Application DTOs.

---

## Clases base (mínimas) por capa

### Dominio (`Domain/Common`)

```csharp
namespace Sastral.Tienda.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
```

```csharp
namespace Sastral.Tienda.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public string? CreatedBy { get; protected set; }
    public DateTime? ModifiedAt { get; protected set; }
    public string? ModifiedBy { get; protected set; }
}
```

Ejemplo de entidad de dominio:

```csharp
using Sastral.Tienda.Domain.Common;

namespace Sastral.Tienda.Domain.Products;

public class Product : BaseAuditableEntity
{
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public Product(string sku, string name, string? description, decimal price)
    {
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        IsActive = true;
    }

    public void Update(string name, string? description, decimal price, bool isActive)
    {
        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive;
        ModifiedAt = DateTime.UtcNow;
    }
}
```

### Aplicación (`Application/Common`, `Application/Abstractions`)

```csharp
namespace Sastral.Tienda.Application.Common;

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool ok, T? value, string? error)
        => (IsSuccess, Value, Error) = (ok, value, error);

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
```

```csharp
namespace Sastral.Tienda.Application.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public long Total { get; init; }
}
```

```csharp
using Sastral.Tienda.Domain.Common;

namespace Sastral.Tienda.Application.Abstractions;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(T entity, CancellationToken ct);
    void Update(T entity);
    void Remove(T entity);
}
```

```csharp
namespace Sastral.Tienda.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
```

DTOs del feature `Products`:

```csharp
namespace Sastral.Tienda.Application.Features.Products.DTOs;

public record CreateProductDto(string Sku, string Name, string? Description, decimal Price);
public record UpdateProductDto(Guid Id, string Name, string? Description, decimal Price, bool IsActive);
public record ProductResponseDto(Guid Id, string Sku, string Name, string? Description, decimal Price, bool IsActive, DateTime CreatedAt);
```

Validación y mapeo:

```csharp
using FluentValidation;
using Sastral.Tienda.Application.Features.Products.DTOs;

namespace Sastral.Tienda.Application.Features.Products.Validation;

public sealed class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

```csharp
using AutoMapper;
using Sastral.Tienda.Application.Features.Products.DTOs;
using Sastral.Tienda.Domain.Products;

namespace Sastral.Tienda.Application.Features.Products.Mappings;

public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, ProductResponseDto>();
    }
}
```

### Infraestructura (`Infrastructure/Persistence`, `Infrastructure/DependencyInjection.cs`)

```csharp
using Microsoft.EntityFrameworkCore;
using Sastral.Tienda.Domain.Products;

namespace Sastral.Tienda.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Sku).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Sku).IsUnique();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });
    }
}
```

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sastral.Tienda.Application.Abstractions;
using Sastral.Tienda.Infrastructure.Persistence;

namespace Sastral.Tienda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(cfg.GetConnectionString("SqlServer")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        // Registrar repositorios e integraciones aquí
        return services;
    }
}
```

### API (`Api/Program.cs` mínimo)

```csharp
using Microsoft.AspNetCore.Mvc;
using Sastral.Tienda.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

## Notas de uso

- Repetir esta estructura por servicio (`Sastral.Tienda`, `Sastral.Gestion`).
- Empezar con CRUD básico (Fase 1) y extender con mensajería/integraciones en fases siguientes.
- Si se prefiere, ubicar contratos públicos en `Api/Contracts` para separar API de `Application`.

---

## Arquitectura de referencia y decisiones

### Arquitectura Limpia (Clean Architecture)

- Límite claro de dependencias: `Domain` no referencia a `Application`, `Infrastructure` ni `Api`. `Application` sólo depende de interfaces en `Abstractions`. `Infrastructure` implementa esas interfaces. `Api` orquesta y no contiene lógica de negocio.
- Beneficios: testabilidad (mocks de `IRepository`), reemplazo de detalles (EF/SQL Server, RabbitMQ) sin tocar reglas, y contratos explícitos entre capas.
- Motivo: priorizar el modelo de negocio sobre frameworks, evitando acoplamientos accidentales que encarecen el cambio.
- Consecuencias: más clases y capas de abstracción al inicio; mayor claridad y evolución segura a mediano plazo.
- Trade-offs: velocidad inicial levemente menor vs. mantenibilidad, testabilidad y extensibilidad superiores.

Aplicación en carpetas:
- `Sastral.Tienda.Domain/*`: modelos y reglas (ej.: `Product : BaseAuditableEntity`).
- `Sastral.Tienda.Application/*`: DTOs, validaciones, mapeos y casos de uso por feature (ej.: `Features/Products`).
- `Sastral.Tienda.Infrastructure/*`: `AppDbContext`, repositorios, integraciones.
- `Sastral.Tienda.Api/*`: transporte HTTP y configuración cross-cutting.

### Arquitectura en capas (capas y responsabilidades)

- Presentación (API): endpoints, autenticación/autorización, serialización, ProblemDetails, Swagger. Carpeta: `Sastral.Tienda.Api`.
- Aplicación: casos de uso coordinando dominio e infraestructura mediante abstracciones; DTOs para no exponer entidades. Carpeta: `Sastral.Tienda.Application`.
- Dominio: entidades, value objects, invariantes. Carpeta: `Sastral.Tienda.Domain`.
- Infraestructura: persistencia (EF Core + configuraciones), messaging, integraciones externas, IoC. Carpeta: `Sastral.Tienda.Infrastructure`.

Por qué: separación de responsabilidades, cambio controlado, y claridad de dependencias.
Motivo: facilitar el diagnóstico de fallos y el trabajo paralelo por equipos en áreas bien delimitadas.
Consecuencias: cierta sobrecarga en flujos simples (pasos entre capas); necesidad de estandarizar DTOs y mapeos.
Trade-offs: simplicidad de un enfoque monolítico vs. orden y escalabilidad del enfoque por capas; elegimos claridad y pruebas predecibles.

### Arquitectura mínima (MVP)

- API: Controllers o Minimal APIs con Swagger y ProblemDetails.
- Application: una carpeta `Features/` por caso de uso relevante (Products, Orders), con DTOs, validaciones, mapeos.
- Domain: entidades esenciales (`Product`, `Order`, `OrderItem`).
- Infrastructure: `AppDbContext`, configuraciones mínimas (claves, índices), repositorio simple y `IUnitOfWork`. Mensajería/asíncrono se incorpora en fases 2-3.

Motivo: acelerar el delivery del MVP con una base extensible hacia mensajería, integraciones y seguridad avanzada.
Consecuencias: posponemos patrones avanzados (outbox, idempotencia, retries) a fases 2-3; impacto controlado por límites claros.
Trade-offs: time-to-market más rápido vs. robustez inmediata; mitigación con diseño preparado para incorporar mejoras sin refactors masivos.

### Decisiones arquitectónicas

- DTOs en `Application/Features/.../DTOs` y opcionalmente `Api/Contracts` si se requiere aislar contratos públicos.
- Repositorios finos por agregado en lugar de un genérico omnipresente, para evitar filtrados pobres y fugar reglas del dominio.
- `Result<T>` y `PagedResult<T>` en `Application/Common` para estandarizar resultados y paginación.
- `DependencyInjection.AddInfrastructure` centraliza wiring por entorno.
- EF Core 9 + SQL Server 2022: madurez, tooling y compatibilidad.

Consecuencias:
- Bajo acoplamiento entre reglas y detalles técnicos.
- Curva de entrada simple para CRUDs; escalable para eventos e integraciones.
Trade-offs: EF Core maximiza productividad pero puede ocultar costes de consulta; mitigamos con proyecciones a DTO, `AsNoTracking`, índices y revisión de planes.

### Patrones utilizados

- Repository/UoW: `IRepository<T>`, `IUnitOfWork` para encapsular persistencia y transacciones.
- DTO + Mapper: separa contratos de transporte y entidades; `ProductProfile` con AutoMapper.
- Validator (FluentValidation): validación declarativa y reutilizable.
- ProblemDetails (RFC 7807): manejo consistente de errores en API.
- Dependency Injection: registro en `DependencyInjection.cs` para sustituir implementaciones.
- Split-by-feature: `Application/Features/<Feature>` agrupa todo lo relacionado a un caso de uso.

Opcionales (fases 2-3):
- Outbox/Inbox para consistencia eventual en eventos.
- Idempotencia en consumidores y DLQ (RabbitMQ).
- CQRS ligero (separar Commands/Queries por folder ya está contemplado).

Motivos y consecuencias por patrón:
- Repository/UoW: Motivo: encapsular persistencia y transacciones; Consecuencia: capa adicional y mocks más simples; mejora pruebas y evita fugas de EF.
- DTO + Mapper: Motivo: contratos estables y desacople del dominio; Consecuencia: costo de mapeo; mitigación con proyecciones directas desde EF.
- Validator: Motivo: reglas de entrada declarativas y reutilizables; Consecuencia: más clases; organizamos por feature para mantener orden.
- ProblemDetails: Motivo: DX y observabilidad consistentes; Consecuencia: estandarizar el manejo de errores en toda la API.

### Estándares adoptados

- API REST
  - Versionado en ruta `/api/v1/...`.
  - Recursos en plural, ids GUID.
  - Paginación `page`, `pageSize` y cabecera `X-Total-Count`.
  - Errores con ProblemDetails y `traceId`.
- Seguridad
  - JWT (claims `sub`, `role`, `jti`) con validación `iss`/`aud` y HSTS/TLS 1.3.
  - Roles mínimos: Admin/Operador (políticas por endpoint).
- Persistencia (EF/SQL)
  - `DbContext` por servicio; `AsNoTracking` para lecturas.
  - Claves foráneas explícitas, índices en FK y campos de búsqueda, `Sku`/`Barcode` únicos donde aplique.
  - Transacciones ACID por caso de uso que muta estado.
- Observabilidad
  - Logs estructurados, health checks, y `CorrelationId` por request.

Por qué: reduce ambigüedad, facilita debugging y garantiza contratos estables desde el MVP.
Consecuencias y trade-offs:
- Versionado REST previene breaking changes pero exige mantener múltiples rutas/documentación.
- JWT simplifica a clientes pero requiere rotación de claves y expiración corta; mitigamos con `kid`, validación `iss/aud` y políticas de rol.
- Índices/constraints mejoran lecturas y consistencia con costo en escrituras; se monitorizan planes y métricas.
- Logs/health checks agregan costo mínimo operativo a cambio de diagnósticos rápidos.

### Ejemplo aplicado (Create Product)

1) `POST /api/v1/products` (API) -> Controller recibe `CreateProductDto`.
2) (Application) Valida con `CreateProductValidator`, mapea a `Product` (AutoMapper) y usa `IRepository<Product>` + `IUnitOfWork`.
3) (Infrastructure) Persiste vía `AppDbContext`, commit transaccional.
4) (API) Retorna `ProductResponseDto` y `201 Created`.

Riesgos y mitigaciones
- Lógica colándose en controladores: controladores finos que delegan en Application.
- Acoplamiento a EF en Application: uso estricto de interfaces y proyecciones; `DbContext` sólo en Infrastructure.
- N+1 y consultas costosas: `Include` selectivo, proyecciones, índices y revisión de planes de ejecución.
- Contratos inestables: versionado de rutas y separación DTOs internos vs. Contracts públicos cuando aplique.

