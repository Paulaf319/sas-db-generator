# Sastral Commerce - Generador de Base de Datos

## Descripción del Proyecto

Sastral Commerce es una solución integral de comercio electrónico diseñada específicamente para negocios de indumentaria que buscan digitalizar su gestión. Este proyecto constituye el generador de base de datos que implementa el modelo de datos completo del sistema utilizando Entity Framework Core 9 con enfoque Code First.

La plataforma unifica dos componentes principales:
- **Tienda Online**: Interfaz web pública para atraer y vender a clientes
- **Aplicación Administrativa**: Herramienta completa para gestionar catálogo, precios, stock, pedidos, caja y auditorías

El objetivo es proporcionar a cualquier empresa, desde una tienda física hasta una cadena de franquicias, las herramientas necesarias para gestionar su negocio de manera eficiente y sin complicaciones técnicas.

## Tecnologías Utilizadas

### Stack Principal
- **.NET 9**: Framework principal con las últimas características y mejoras de rendimiento
- **Entity Framework Core 9**: ORM moderno para .NET con soporte completo para Code First
- **SQL Server 2022**: Base de datos empresarial con características avanzadas de rendimiento y seguridad
- **C# 12**: Lenguaje de programación con características modernas y sintaxis expresiva

### Paquetes NuGet Implementados
- **Microsoft.EntityFrameworkCore (9.0.0)**: Funcionalidad core de Entity Framework
- **Microsoft.EntityFrameworkCore.SqlServer (9.0.0)**: Provider específico para SQL Server
- **Microsoft.EntityFrameworkCore.Tools (9.0.0)**: Herramientas de migración para Package Manager Console
- **Microsoft.EntityFrameworkCore.Design (9.0.0)**: Herramientas de diseño para migraciones
- **Microsoft.Extensions.Configuration (9.0.0)**: Sistema de configuración flexible
- **Microsoft.Extensions.DependencyInjection (9.0.0)**: Contenedor de inyección de dependencias
- **Microsoft.Extensions.Logging (9.0.0)**: Framework de logging estructurado

## Principios Arquitectónicos y Decisiones de Diseño

### Clean Architecture (Arquitectura Limpia)

El proyecto implementa Clean Architecture para garantizar:

**Separación Clara de Responsabilidades:**
- **Dominio (Domain)**: Contiene las entidades de negocio, value objects y reglas invariantes sin dependencias externas
- **Aplicación (Application)**: Define contratos e interfaces para casos de uso sin implementaciones concretas
- **Infraestructura (Infrastructure)**: Implementa las interfaces de aplicación y maneja persistencia e integraciones
- **Consola (Console)**: Punto de entrada para la generación y gestión de la base de datos

**Beneficios Obtenidos:**
- **Testabilidad**: Cada capa puede probarse independientemente usando mocks
- **Mantenibilidad**: Cambios en una capa no afectan otras capas
- **Flexibilidad**: Fácil reemplazo de implementaciones (ej: cambiar de SQL Server a PostgreSQL)
- **Escalabilidad**: Estructura preparada para crecimiento y nuevas funcionalidades

### Principios SOLID Aplicados

**1. Single Responsibility Principle (SRP)**
- Cada entidad tiene una única razón para cambiar
- Separación clara entre lógica de dominio y persistencia
- Configuraciones de entidades aisladas en clases específicas

**2. Open/Closed Principle (OCP)**
- Entidades abiertas para extensión mediante herencia de BaseEntity/BaseAuditableEntity
- Cerradas para modificación mediante encapsulación de propiedades

**3. Liskov Substitution Principle (LSP)**
- Jerarquía consistente de entidades base
- Polimorfismo respetado en todas las implementaciones

**4. Interface Segregation Principle (ISP)**
- Interfaces específicas (IRepository<T>, IUnitOfWork) en lugar de interfaces monolíticas
- Contratos mínimos y cohesivos

**5. Dependency Inversion Principle (DIP)**
- Dependencias hacia abstracciones, no hacia implementaciones concretas
- Inyección de dependencias para desacoplar capas

### Domain-Driven Design (DDD)

**Agregados Bien Definidos:**
- **Usuario**: Gestión de identidad y roles
- **Producto**: Catálogo con variantes y categorías
- **Pedido**: Proceso de compra completo
- **Carrito**: Gestión de sesión de compra
- **Pago**: Integración con proveedores de pago
- **Envío**: Logística y seguimiento
- **Inventario**: Control de stock y movimientos
- **Auditoría**: Trazabilidad de operaciones críticas

**Encapsulación de Lógica de Negocio:**
- Métodos de dominio que garantizan invariantes
- Constructores privados para EF Core
- Propiedades privadas con setters para mantener consistencia

### Patrones de Diseño Implementados

**Repository Pattern:**
- Abstracción de acceso a datos mediante IRepository<T>
- Operaciones CRUD genéricas y extensibles
- Separación entre lógica de consulta y persistencia

**Unit of Work Pattern:**
- Gestión transaccional mediante IUnitOfWork
- Garantía de consistencia en operaciones múltiples
- Control centralizado de cambios en el contexto

**Configuration Pattern:**
- Configuraciones de entidades separadas por responsabilidad
- Aplicación automática mediante reflexión
- Mantenimiento simplificado de mapeos

### Decisiones Técnicas Específicas

**Uso de GUIDs como Claves Primarias:**
- **Ventajas**: Distribución, seguridad, unicidad global
- **Consideración**: Ligero impacto en rendimiento mitigado con índices apropiados

**Enums como Strings:**
- **Ventaja**: Legibilidad en base de datos y facilidad de debugging
- **Consideración**: Espacio adicional compensado por claridad y mantenibilidad

**Soft Delete mediante IsActive:**
- **Beneficio**: Preservación de datos históricos y referencias
- **Implementación**: Filtros globales para consultas automáticas

**Auditoría Integrada:**
- **BaseAuditableEntity**: Tracking automático de creación y modificación
- **AuditLog**: Registro detallado de operaciones sensibles
- **Trazabilidad**: Correlación por usuario e IP para investigaciones

**Índices Estratégicos:**
- Claves foráneas para optimizar joins
- Campos de búsqueda frecuente (SKU, Barcode, Email)
- Campos de filtrado temporal (CreatedAt)

La arquitectura implementa una separación clara de responsabilidades en cuatro capas principales:

```
┌─────────────────────────────────────────────────────────────┐
│                    CONSOLE APPLICATION                      │
│                  (Punto de Entrada)                        │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                  INFRASTRUCTURE LAYER                      │
│              (EF Core, Configuraciones, DI)                │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                  APPLICATION LAYER                         │
│              (Interfaces, Abstracciones)                   │
└─────────────────────────┬───────────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────────┐
│                    DOMAIN LAYER                            │
│            (Entidades, Lógica de Negocio)                  │
└─────────────────────────────────────────────────────────────┘
```

### Flujo de Dependencias
- **Console** → **Infrastructure** → **Application** → **Domain**
- Las dependencias siempre apuntan hacia adentro (hacia el dominio)
- El dominio no conoce las capas externas (inversión de dependencias)

## Requisitos Previos

- **.NET 9 SDK**: Framework de desarrollo más reciente
- **SQL Server 2022** (o SQL Server LocalDB): Base de datos objetivo
- **Entity Framework Core Tools**: Para gestión de migraciones

## Paquetes NuGet Incluidos

Todos los paquetes necesarios ya están incluidos en el proyecto con las versiones más recientes compatibles con .NET 9:

### Paquetes Core de Entity Framework
- **Microsoft.EntityFrameworkCore (9.0.0)**: Funcionalidad principal del ORM
- **Microsoft.EntityFrameworkCore.SqlServer (9.0.0)**: Provider específico para SQL Server
- **Microsoft.EntityFrameworkCore.Tools (9.0.0)**: Herramientas CLI para migraciones
- **Microsoft.EntityFrameworkCore.Design (9.0.0)**: Herramientas de tiempo de diseño

### Paquetes de Extensiones Microsoft
- **Microsoft.Extensions.Configuration (9.0.0)**: Sistema de configuración
- **Microsoft.Extensions.Configuration.Json (9.0.0)**: Soporte para archivos JSON
- **Microsoft.Extensions.DependencyInjection (9.0.0)**: Contenedor IoC
- **Microsoft.Extensions.Logging (9.0.0)**: Framework de logging

## Modelo de Datos Completo

El sistema implementa un modelo de datos robusto que cubre todos los aspectos de un e-commerce moderno:

### 👥 Gestión de Usuarios
- **User**: Usuarios del sistema con autenticación y roles
  - Propiedades: Email, PasswordHash, RoleId, IsActive
  - Relaciones: Role, Orders, Carts, AuditLogs
- **Role**: Roles de usuario con permisos diferenciados
  - Roles predefinidos: Admin, Operador
  - Extensible para nuevos roles según necesidades del negocio

### 🛍️ Catálogo de Productos
- **Product**: Información principal de productos
  - Propiedades: SKU, Name, Description, Price, CategoryId, BrandId
  - Soporte para marcas y categorización jerárquica
- **ProductVariant**: Variaciones de productos (color, talla, stock)
  - Propiedades: Color, Size, Barcode, Stock
  - Control granular de inventario por variante
- **Category**: Categorías con estructura jerárquica
  - Soporte para subcategorías ilimitadas
  - Navegación eficiente del catálogo

### 🛒 Compras y Pedidos
- **Cart**: Carritos de compra para usuarios
  - Estados: Active, Abandoned, Converted
  - Soporte para carritos anónimos y de usuarios registrados
- **CartItem**: Artículos en carritos de compra
- **Order**: Pedidos de clientes con seguimiento completo
  - Estados: Draft, Confirmed, Paid, Shipped, Delivered, Cancelled
  - Tracking de estado de pago y envío independientes
- **OrderItem**: Artículos individuales en pedidos

### 💳 Pagos y Envíos
- **Payment**: Transacciones de pago con integración de proveedores
  - Proveedores: MercadoPago, Stripe, Cash
  - Estados: Pending, Processing, Approved, Rejected, Cancelled, Refunded
- **Shipment**: Información de envío y seguimiento
  - Proveedores: MercadoEnvios, Correo, Pickup
  - Tracking completo con fechas estimadas y reales

### 📦 Inventario y Auditoría
- **InventoryMovement**: Seguimiento de movimientos de stock
  - Razones: Purchase, Sale, Adjustment, Return, Damage, Loss, Transfer
  - Trazabilidad completa de cambios en inventario
- **AuditLog**: Registro de auditoría para operaciones sensibles
  - Captura: Usuario, Acción, Entidad, Datos antes/después, IP
  - Cumplimiento normativo y investigación de incidentes

## Configuración del Sistema

### Cadena de Conexión

Actualice la cadena de conexión en `src/Sastral.DbGenerator.Console/appsettings.json` según su entorno:

#### Para SQL Server Local
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SastralCommerceDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### Para SQL Server Remoto
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=SastralCommerceDb;User Id=your_user;Password=your_password;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### Para SQL Server en Docker
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=SastralCommerceDb;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Configuración de Logging

El sistema incluye logging estructurado configurable:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## Guía de Uso

### 1. Compilar la Solución
```bash
# Compilar todos los proyectos
dotnet build

# Verificar que no hay errores de compilación
dotnet build --verbosity normal
```

### 2. Crear Migración Inicial
```bash
# Crear la primera migración con todas las entidades
dotnet ef migrations add InitialCreate \
  --project src/Sastral.DbGenerator.Infrastructure \
  --startup-project src/Sastral.DbGenerator.Console \
  --verbose
```

### 3. Actualizar Base de Datos
```bash
# Aplicar migraciones a la base de datos
dotnet ef database update \
  --project src/Sastral.DbGenerator.Infrastructure \
  --startup-project src/Sastral.DbGenerator.Console \
  --verbose
```

### 4. Ejecutar el Generador de Base de Datos
```bash
# Ejecutar la aplicación consola
dotnet run --project src/Sastral.DbGenerator.Console

# O ejecutar con configuración específica
dotnet run --project src/Sastral.DbGenerator.Console --configuration Release
```

### 5. Comandos Adicionales Útiles

#### Verificar Estado de Migraciones
```bash
dotnet ef migrations list \
  --project src/Sastral.DbGenerator.Infrastructure \
  --startup-project src/Sastral.DbGenerator.Console
```

#### Generar Script SQL
```bash
dotnet ef migrations script \
  --project src/Sastral.DbGenerator.Infrastructure \
  --startup-project src/Sastral.DbGenerator.Console \
  --output migration.sql
```

#### Revertir Migración
```bash
dotnet ef database update PreviousMigrationName \
  --project src/Sastral.DbGenerator.Infrastructure \
  --startup-project src/Sastral.DbGenerator.Console
```

## Estructura del Proyecto

```
db-generator/
├── src/                                      # Código fuente
│   ├── Sastral.DbGenerator.Domain/           # 🏛️ CAPA DE DOMINIO
│   │   ├── Common/                           # Entidades base
│   │   │   ├── BaseEntity.cs                 # Entidad base con Id
│   │   │   └── BaseAuditableEntity.cs        # Entidad con auditoría
│   │   └── Entities/                         # Entidades de dominio
│   │       ├── Users/                        # 👥 Gestión de usuarios
│   │       │   ├── User.cs                   # Usuario del sistema
│   │       │   └── Role.cs                   # Roles y permisos
│   │       ├── Products/                     # 🛍️ Catálogo de productos
│   │       │   ├── Product.cs                # Producto principal
│   │       │   ├── ProductVariant.cs         # Variantes (color, talla)
│   │       │   └── Category.cs               # Categorías jerárquicas
│   │       ├── Orders/                       # 📋 Gestión de pedidos
│   │       │   ├── Order.cs                  # Pedido principal
│   │       │   └── OrderItem.cs              # Artículos del pedido
│   │       ├── Carts/                        # 🛒 Carritos de compra
│   │       │   ├── Cart.cs                   # Carrito principal
│   │       │   └── CartItem.cs               # Artículos del carrito
│   │       ├── Payments/                     # 💳 Procesamiento de pagos
│   │       │   └── Payment.cs                # Transacciones de pago
│   │       ├── Shipments/                    # 📦 Gestión de envíos
│   │       │   └── Shipment.cs               # Información de envío
│   │       ├── Inventory/                    # 📊 Control de inventario
│   │       │   └── InventoryMovement.cs      # Movimientos de stock
│   │       └── Audits/                       # 🔍 Registro de auditoría
│   │           └── AuditLog.cs               # Logs de operaciones
│   │
│   ├── Sastral.DbGenerator.Application/      # 🔧 CAPA DE APLICACIÓN
│   │   └── Abstractions/                     # Interfaces y contratos
│   │       ├── IRepository.cs                # Patrón Repository
│   │       └── IUnitOfWork.cs                # Patrón Unit of Work
│   │
│   ├── Sastral.DbGenerator.Infrastructure/   # 🏗️ CAPA DE INFRAESTRUCTURA
│   │   ├── Persistence/
│   │   │   ├── Contexts/                     # Contextos de EF Core
│   │   │   │   └── SastralDbContext.cs       # DbContext principal
│   │   │   └── Configurations/               # Configuraciones de entidades
│   │   │       ├── UserConfiguration.cs      # Config. de User
│   │   │       ├── ProductConfiguration.cs   # Config. de Product
│   │   │       └── [Otras configuraciones]   # Una por entidad
│   │   └── DependencyInjection.cs           # Registro de servicios
│   │
│   └── Sastral.DbGenerator.Console/          # 🖥️ APLICACIÓN CONSOLA
│       ├── Program.cs                        # Punto de entrada
│       └── appsettings.json                  # Configuración
│
├── Sastral.DbGenerator.sln                   # Archivo de solución
└── README.md                                 # Esta documentación
```

### Descripción de Capas

#### 🏛️ Capa de Dominio (Domain)
- **Responsabilidad**: Contiene la lógica de negocio pura y las entidades del dominio
- **Características**: Sin dependencias externas, reglas de negocio encapsuladas
- **Beneficios**: Testeable, reutilizable, independiente de frameworks

#### 🔧 Capa de Aplicación (Application)  
- **Responsabilidad**: Define contratos e interfaces para casos de uso
- **Características**: Abstracciones sin implementaciones concretas
- **Beneficios**: Desacoplamiento, facilita testing con mocks

#### 🏗️ Capa de Infraestructura (Infrastructure)
- **Responsabilidad**: Implementa interfaces de aplicación y maneja persistencia
- **Características**: Entity Framework, configuraciones, migraciones
- **Beneficios**: Intercambiable, configurable, optimizable

#### 🖥️ Aplicación Consola (Console)
- **Responsabilidad**: Punto de entrada para generación de base de datos
- **Características**: Configuración, logging, orquestación
- **Beneficios**: Automatizable, scripteable, observable

## Características Principales

### 🏗️ Arquitectura y Diseño
- **Clean Architecture**: Separación adecuada de responsabilidades por capas
- **Principios SOLID**: Código mantenible, extensible y testeable
- **Domain-Driven Design**: Modelo rico del dominio con lógica de negocio encapsulada
- **Patrones Probados**: Repository, Unit of Work, Configuration

### 🚀 Tecnología de Vanguardia
- **Entity Framework Core 9**: Últimas características y mejoras de rendimiento
- **Code First Approach**: Esquema de base de datos generado desde código
- **SQL Server 2022**: Base de datos empresarial con características avanzadas
- **.NET 9**: Framework más reciente con optimizaciones de rendimiento

### 📊 Modelo de Datos Completo
- **E-commerce Integral**: Entidades completas para comercio electrónico
- **Auditoría Integrada**: Seguimiento automático de operaciones sensibles
- **Flexibilidad**: Fácil personalización y extensión según necesidades
- **Rendimiento**: Índices estratégicos y optimizaciones de consulta

### 🔧 Facilidad de Uso
- **Configuración Flexible**: Múltiples entornos soportados (Local, Remoto, Docker)
- **Logging Estructurado**: Diagnóstico y monitoreo integrados
- **Migraciones Automáticas**: Evolución controlada del esquema de base de datos
- **Documentación Completa**: Guías detalladas y ejemplos prácticos

## Próximos Pasos

Después de generar la base de datos, puede proceder con:

### 1. Integración con APIs
- Utilizar este modelo en proyectos de API Web
- Implementar controladores REST sobre las entidades
- Configurar autenticación y autorización JWT

### 2. Implementación de Repositorios
- Crear implementaciones concretas de IRepository<T>
- Agregar métodos de consulta específicos del dominio
- Implementar patrones de consulta optimizados

### 3. Lógica de Aplicación
- Desarrollar casos de uso en la capa Application
- Implementar validaciones con FluentValidation
- Agregar mapeo automático con AutoMapper

### 4. Validación y Mapeo
- Crear DTOs para transferencia de datos
- Implementar validaciones de entrada robustas
- Configurar perfiles de mapeo entre entidades y DTOs

### 5. Endpoints de API
- Crear controladores para cada agregado
- Implementar operaciones CRUD completas
- Agregar funcionalidades específicas del negocio

## Consideraciones Importantes

### 🔒 Seguridad
- La base de datos incluye datos semilla para roles predeterminados (Admin, Operador)
- Todas las entidades siguen el patrón BaseEntity/BaseAuditableEntity
- Las contraseñas deben hashearse antes del almacenamiento
- Implementar validación de entrada en todas las capas

### ⚡ Rendimiento
- Las relaciones de clave foránea están configuradas correctamente
- Se agregan índices en campos clave para rendimiento
- Los valores enum se almacenan como strings para legibilidad
- Usar AsNoTracking() para consultas de solo lectura

### 🔄 Mantenimiento
- Las migraciones permiten evolución controlada del esquema
- La auditoría integrada facilita el diagnóstico de problemas
- La estructura modular permite actualizaciones incrementales
- El logging estructurado ayuda en la resolución de incidentes

### 📈 Escalabilidad
- La arquitectura soporta crecimiento horizontal
- Las entidades están diseñadas para distribución
- Los patrones implementados facilitan la extensión
- La separación de capas permite optimizaciones específicas

## Soporte y Contribuciones

### 📞 Contacto
Para consultas técnicas o soporte, puede contactar al equipo de desarrollo a través de los canales oficiales del proyecto.

### 🤝 Contribuciones
Las contribuciones son bienvenidas. Por favor, siga las convenciones establecidas y asegúrese de que las pruebas pasen antes de enviar pull requests.

### 📚 Documentación Adicional
- Consulte la documentación técnica completa en `Sastral_Commerce_Documentation.md`
- Revise la estructura del backend en `Sastral_Commerce_Backend_Estructura.md`
- Mantenga actualizada la documentación con los cambios realizados
