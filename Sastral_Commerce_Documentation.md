# Sastral Commerce

## Tabla de Contenidos

- Descripción General
- Versiones objetivo
- Público Objetivo
- Arquitectura del Sistema
- Tecnologías Utilizadas
- Fases de Desarrollo
- Requisitos Funcionales
- Requisitos No Funcionales
- Planificación de Fases
- Consideraciones de Seguridad
- Planes de la Solución
- Mejores Prácticas
- Herramientas Recomendadas
- Diseño Técnico Detallado
- Plan por Fases, Tareas y Subtareas
- Glosario y Convenciones
- Supuestos y Riesgos
- Conclusión

## Descripción General

Sastral Commerce es una solución integral diseñada para negocios de indumentaria que buscan digitalizar su gestión. La plataforma unifica:

- **Tienda Online**: Web pública para atraer y vender a clientes.
- **Aplicación Administrativa**: Herramienta para gestionar el catálogo, precios, stock, pedidos, caja y auditorías.

El objetivo es que cualquier empresa, desde una tienda física hasta una cadena de franquicias, pueda gestionar su negocio de manera eficiente y sin complicaciones.

### Resumen ejecutivo

Sastral Commerce consolida una tienda online y una aplicación de gestión sobre un stack moderno (Angular 18 en frontend, .NET 9 en backend, EF Core 9 y SQL Server 2022) para digitalizar el negocio de indumentaria. Se adopta Clean Architecture, APIs REST, mensajería con RabbitMQ y despliegue en AWS, priorizando desde el MVP los flujos de pagos y envíos, y la integración con Mercado Libre.

La hoja de ruta se divide en cuatro fases que incrementan capacidades: CRUDs y autenticación; checkout y envíos simples; logística con Mercado Envíos y conciliación de pagos; y finalmente almacenamiento de imágenes en Azure Blob con mejoras operativas y de seguridad.

## Público Objetivo

- **Tiendas Físicas**: Negocios que desean digitalizar su gestión interna.
- **Tiendas Online**: Empresas que buscan expandir su presencia en el comercio electrónico.
- **Franquicias**: Cadenas que requieren una gestión centralizada y eficiente.

## Arquitectura del Sistema

El sistema se desarrollará utilizando una arquitectura de microservicios, dividida en los siguientes módulos y capas, con Clean Architecture y principios SOLID para asegurar bajo acoplamiento y alta cohesión:

1. **Frontend**: Aplicación desarrollada con Angular 18, estructura modular por dominio (tienda/gestión), lazy loading, interceptores para JWT, manejo de estado básico reactivo y componentes reutilizables.
2. **Servicio Tienda**: Backend desarrollado con .NET 9. Capas API (controllers/minimal APIs, DTOs, ProblemDetails), Aplicación (casos de uso y validaciones), Dominio (entidades/value objects) e Infraestructura (EF Core, repositorios, integraciones, mensajería).
3. **Servicio Gestión**: Backend desarrollado con .NET 9, con la misma estructura de capas, orientado a administración de catálogo, stock, usuarios/roles, auditoría y reportes.

Se aplicarán principios SOLID y una arquitectura limpia por capas para garantizar un código mantenible y escalable. Comunicación síncrona (REST/JSON) y asíncrona (RabbitMQ a partir de Fase 2/3).

## Tecnologías Utilizadas

- **Frontend**: Angular 18 (Node.js 20 LTS recomendado).
- **Backend**: .NET 9.
- **Base de Datos**: SQL Server 2022, utilizando Entity Framework Core 9 para la persistencia de datos con migraciones, `DbContext` por servicio y convenciones de nombres consistentes.
- **Almacenamiento de Imágenes**: Azure Blob Storage.
- **Integraciones**: APIs de Mercado Libre para catálogo/pedidos, Mercado Pago para pagos, Mercado Envíos para logística.
- **Autenticación**: Implementación de JWT para la autenticación de usuarios, roles mínimos (Admin/Operador) y políticas por endpoint. TLS 1.3 y HSTS habilitados.
- **Despliegue**: AWS para el despliegue de la aplicación.

## Fases de Desarrollo

El desarrollo del MVP se planifica en cuatro fases:

1. **Fase 1**: Proyectos base, base de datos y APIs REST básicas con CRUDs simples sin lógica; autenticación JWT básica y semilla de roles/usuario admin.
2. **Fase 2 (Post-MVP-1)**: Pagos (Mercado Pago), envíos simples, eventos básicos (RabbitMQ), despliegue de prueba en AWS.
3. **Fase 3 (Post-MVP-2)**: Mercado Envíos, conciliación de pagos, sincronización con Mercado Libre, auditoría robusta y consumidores idempotentes.
4. **Fase 4 (Post-MVP-3)**: Azure Blob Storage para imágenes, hardening de seguridad, mejoras de rendimiento y CI/CD maduro.

## Versiones objetivo

- Angular 18 (Node.js 20 LTS)
- .NET 9 / ASP.NET Core 9
- Entity Framework Core 9
- SQL Server 2022
- RabbitMQ 3.13.x
- TLS 1.3

## Requisitos Funcionales

- **Tienda Online**:
  - Catálogo de productos.
  - Carrito de compras.
  - Procesamiento de pagos (Mercado Pago) con webhooks y conciliación básica.
  - Gestión de pedidos con estados (Draft/Confirmed/Paid/Shipped/Cancelled).
  - Integración con Mercado Libre (sincronización de catálogo y pedidos en fases 2-3).

- **Aplicación Administrativa**:
  - Gestión de catálogo.
  - Control de stock.
  - Gestión de pedidos.
  - Administración de caja (ingresos/egresos simples y reportes básicos).
  - Auditorías (registro de acciones sensibles con datos antes/después).

## Requisitos No Funcionales

- **Seguridad**: Implementación de JWT para autenticación y cumplimiento de las reglas de seguridad requeridas por Mercado Libre para sus APIs.
- **Pruebas**: Implementación de pruebas unitarias con xUnit en .NET y Jasmine/Karma en Angular; pruebas de integración de API con TestServer; smoke tests post-despliegue.
- **Despliegue**: Uso de AWS para el despliegue de la aplicación con entorno de pruebas; CI/CD automatizado.
- **Observabilidad**: Logs estructurados, métricas básicas y alertas de errores.
- **Mantenibilidad**: Estándares de nombres, estructura por capas, linters y formateadores.

## Planificación de Fases

- **Fase 1**:
  - Configuración de proyectos base.
  - Diseño de la base de datos.
  - Implementación de APIs REST básicas con CRUDs simples.

- **Fase 2**:
  - Pago/checkout con Mercado Pago (Checkout Redirect).
  - Envíos con costo fijo y captura de dirección.
  - RabbitMQ inicial y notificaciones.
  - Integración inicial con Mercado Libre para publicar ítems.

- **Fase 3**:
  - Mercado Envíos (cotización, etiquetas, tracking).
  - Conciliación de pagos y manejo de reintentos.
  - Importación de pedidos desde Mercado Libre y sincronización de stock/precio.
  - Auditoría completa y reportes básicos.

- **Fase 4**:
  - Azure Blob Storage para imágenes (a través de interfaz de almacenamiento).
  - Hardening de seguridad, métricas básicas y logging estructurado.
  - Optimización de consultas e índices.
  - CI/CD con promoción por ambientes.

## Consideraciones de Seguridad

- **Autenticación y Autorización**: Implementación de JWT para la autenticación de usuarios, validación de `aud`/`iss`, rotación de claves (`kid`) y roles mínimos.
- **Protección de Datos**: Uso de Azure Blob Storage para el almacenamiento seguro de imágenes y control de acceso a URLs firmadas.
- **Cumplimiento de Normativas**: Asegurar el cumplimiento de las reglas de seguridad requeridas por Mercado Libre y Mercado Pago para sus APIs (verificación de firmas, scopes de OAuth donde aplique).
- **Aplicación Web**: CORS explícito, mitigación de XSS/CSRF en frontend, validación exhaustiva en backend.

## Planes de la Solución

Se contemplan diferentes planes según las necesidades de cada cliente:

- **Plan Administrativo**: Solo la aplicación de gestión, con todo lo requerido para administrar un local físico.
- **Plan Tienda Online**: Solo la tienda en la nube, enfocada en ventas digitales.
- **Plan Full**: Combinación de ambos productos al 100%, integrados en una sola solución.

## Mejores Prácticas

- **Angular**: Modularización por dominio, lazy loading, interceptores JWT, `ReactiveForms`, detección de cambios OnPush, componentes standalone donde convenga.
- **.NET**: Aplicar principios SOLID y Clean Architecture; validación con FluentValidation; mapeo con Mapster/AutoMapper; logging estructurado (Serilog) y ProblemDetails.
- **Entity Framework**: Utilizar migraciones, `AsNoTracking` para lecturas, `Include` selectivo, paginación server-side y restricciones únicas.
- **SQL Server**: Optimizar consultas, índices por FK y columnas de búsqueda, planes de ejecución revisados y estadísticas actualizadas.
- **Microservicios**: Servicios independientes con contratos claros, idempotencia en consumidores y reintentos con backoff; timeouts y circuit breakers para externos.
- **Pruebas**: xUnit para .NET, Jasmine/Karma para Angular, integración de API y e2e básicos post-Fase 2.

## Herramientas Recomendadas

- **Editores de Markdown**: Para la documentación, se recomienda el uso de editores como Visual Studio Code con la extensión "Markdown All in One" para facilitar la edición y visualización de archivos Markdown.

## Diseño Técnico Detallado

### Arquitectura y Capas (Clean Architecture + SOLID)

- **Frontend (Angular 18)**: aplicación SPA para Tienda y panel de Gestión. En Fase 1 una sola app con módulos separados; opcional separar en proyectos distintos en fases posteriores.
- **Backend .NET 9 (microservicios)**:
  - `Servicio Tienda` (API pública): catálogo, carrito, pedidos, pagos y envíos.
  - `Servicio Gestión` (API privada/admin): productos, stock, precios, usuarios, auditorías, reportes.
- **Capas por servicio**:
  - `API` (endpoints REST, DTOs, validación, auth JWT)
  - `Aplicación` (casos de uso, orquestación, validaciones de dominio, handlers)
  - `Dominio` (entidades, value objects, reglas)
  - `Infraestructura` (EF Core, repositorios, migraciones, integración externos, mensajería)
- **Comunicación**: síncrona vía REST; asíncrona vía RabbitMQ (eventos de negocio) a partir de Fase 2/3.

### Estándares de API (REST)

- Versionado en URL: `/api/v1/...`.
- Convenciones de recursos: plural, ids GUID/ULID. Ej.: `/api/v1/products/{id}`.
- Paginación: `page`, `pageSize` con `X-Total-Count`.
- Filtros: querystring (`status`, `from`, `to`, etc.).
- Errores: RFC 7807 Problem Details.
- Seguridad: Bearer JWT; CORS explícito.
 - Transporte: HTTPS con TLS 1.3 y HSTS.
- Errores: usar ProblemDetails con códigos y `traceId`; validación consistente (FluentValidation).

### Entidades del Dominio (mínimas)

- `User` (Id, Email, PasswordHash, Role, IsActive, CreatedAt)
- `Role` (Id, Name) — mínimo: `Admin`, `Operador`
- `Product` (Id, Sku, Name, Description, BrandId?, CategoryId, Price, IsActive)
- `ProductVariant` (Id, ProductId, Color, Size, Barcode, Stock)
- `Category` (Id, Name, ParentId?)
- `InventoryMovement` (Id, VariantId, Qty, Reason, PerformedBy, CreatedAt)
- `Cart` (Id, UserId?, CreatedAt, Status)
- `CartItem` (Id, CartId, VariantId, Qty, UnitPrice)
- `Order` (Id, Number, UserId?, Total, Status, PaymentStatus, ShippingStatus, CreatedAt)
- `OrderItem` (Id, OrderId, VariantId, Qty, UnitPrice)
- `Payment` (Id, OrderId, Provider, ProviderPaymentId, Amount, Status, CreatedAt)
- `Shipment` (Id, OrderId, Provider, TrackingCode, Address, Cost, Status)
- `AuditLog` (Id, UserId, Action, Entity, EntityId, DataBefore, DataAfter, CreatedAt, Ip)

Notas:
- Índices por `Sku`, `Barcode`, `CategoryId`, `CreatedAt`.
- Soft delete donde aplique (bandera `IsActive`).

### Seguridad y Roles

- **JWT** con expiración corta y refresh opcional en fases posteriores. Claims: `sub`, `role`, `jti`.
- Roles mínimos: `Admin` (total) y `Operador` (limitado a operaciones diarias). Política por endpoint.
- Hash de contraseñas con ASP.NET Core Identity (PBKDF2) o BCrypt. MFA opcional post-MVP.

### Auditoría

- Middleware/behaviour en capa Aplicación para registrar acciones críticas: creación/edición/borrado de entidades sensibles (precios, stock, pedidos, usuarios, roles).
- Persistencia en `AuditLog`. Correlación por `CorrelationId` en headers para trazar flujos.

### Mensajería (RabbitMQ) y catálogo de eventos

- Exchange tipo `topic` por dominio; colas por servicio/worker.
- Eventos mínimos (post-Fase 2):
  - `OrderCreated`, `OrderPaid`, `ShipmentRequested`, `InventoryAdjusted`
  - `EmailNotificationRequested`
  - `ProductSyncedWithMeli`, `MeliOrderImported`
- Workers background para: notificaciones email, sincronización Mercado Libre, conciliación de pagos y actualizaciones de stock.

### Integraciones Enfocadas en Pagos y Envíos

- **Pagos (MVP)**: Mercado Pago (preferente en LatAm). Flujos: Checkout Pro/Redirect en Fase 1-2; Webhooks en Fase 2 para `payment.updated`.
- **Pagos (alternativa/expansión)**: Stripe (tokenización y webhooks); seleccionar por configuración.
- **Envíos (MVP)**: costo fijo/reglas simples + datos de envío básicos.
- **Envíos**: Mercado Envíos: cotización, etiquetas y tracking; webhooks de estado.
- **Mercado Libre**: catálogo y pedidos. Jobs para publicar/actualizar ítems y para importar pedidos. Respetar límites de rate y oauth.

### Persistencia (SQL Server + EF Core)

- EF Core 9 (compatible con .NET 9). Migraciones por servicio; `DbContext` por bounded context.
- Convenciones: snake_case o PascalCase consistente; claves primarias `Id` (GUID). Foreign keys explícitas. Índices requeridos.
- Transacciones ACID en casos de uso; uso cuidadoso de `AsNoTracking` para lecturas.
- Semillas mínimas (roles admin, usuario admin) en Fase 1.
- Estándares SQL: claves foráneas explícitas, constraints únicas (`Sku`, `Barcode`), índices por FK y campos de búsqueda, uso de transacciones donde aplique.

### Despliegue y Plataforma

- Contenedores Docker para todos los servicios.
- AWS: EKS (Kubernetes) 1.29+ para orquestación, ECR para imágenes, RDS SQL Server 2022 para base de datos, S3 para artefactos, CloudWatch/Prometheus+Grafana para observabilidad.
- Secrets: AWS Secrets Manager.
- Storage de imágenes: Azure Blob Storage planificado para Fase 4 (no bloqueante del MVP); usar interfaz de almacenamiento para switch provider.

### CI/CD

- GitHub Actions: build, test, análisis (dotnet format/sonar opcional), docker build/push a ECR, despliegue a EKS (Helm/Kustomize) por entorno.

### Pruebas

- Framework: **xUnit** (adoptado por ecosistema .NET, buen soporte de paralelismo y Fixtures; más flexible que NUnit para .NET moderno).
- Pirámide: unitarias (dominio/aplicación), componentes (handlers/repos), integración (API con TestServer), e2e básicos (Postman/Newman) post-Fase 2.
- Cobertura objetivo inicial: >60% en dominio/aplicación para MVP.

### Buenas Prácticas Clave

- **Angular**: módulos por dominio, `standalone components` si aplica, servicios inyectables, `HttpClient` con interceptores JWT, `ReactiveForms`, `BehaviorSubject` para estado (NgRx opcional Fase 3+), lazy loading y preloading.
- **.NET**: validación con FluentValidation, mapeo con Mapster/AutoMapper, registros estructurados (Serilog), `ProblemDetails`, minimal APIs o Controllers; MediatR opcional para CQRS ligera.
- **EF/SQL**: evitar N+1, `Include` selectivo, proyecciones a DTO, migraciones controladas, naming consistente, índices en FK y búsquedas.
- **Microservicios**: contratos claros, idempotencia en consumidores, reintentos con backoff para integraciones, timeouts y circuit breakers.
- **Documentación**: mantener este documento actualizado por fase; changelog corto por versión.

---

## Plan por Fases, Tareas y Subtareas (por Módulo)

Notas generales: Proyecto en carácter de prototipo, sin objetivos de escalabilidad ni SLAs. JWT como esquema de autenticación; se cumplirán requisitos de seguridad de las APIs de Mercado Libre.

### Fase 1 — Proyectos base, DB y APIs CRUD sin lógica

- Frontend (Angular)
  - Crear workspace y estructura base (core/shared/modules tienda/gestión)
  - Routing, layouts, componentes shell, estilos
  - Servicios HTTP y modelos básicos (Product, Variant, Category, Order)
  - Login con JWT (mock/back básico), guardas y roles mínimos
- Servicio Tienda (.NET 9)
  - Proyecto solución con capas (API, Application, Domain, Infrastructure)
  - Entidades base y DbContext; migraciones iniciales SQL Server
  - Endpoints CRUD: Products, Variants, Categories, Carts, Orders
  - Autenticación JWT y roles; seeding de usuario admin
- Servicio Gestión (.NET 9)
  - Estructura por capas similar; entidades administrativas
  - CRUDs: Products, Variants, Prices, InventoryMovements, Users
  - Auditoría mínima (inserción básica `AuditLog`)
- DevOps
  - Dockerfiles para cada servicio y frontend
  - GitHub Actions: build y test; push de imágenes a container registry

Entregables: Apps ejecutables localmente (docker-compose), CRUDs funcionales, auth JWT básica, DB lista.

### Fase 2 — Post-MVP-1

- Pagos y Envíos (prioridad)
  - Integrar Mercado Pago (Checkout Redirect) en Tienda; Webhook receiver en backend
  - Estado de pago en `Order` y creación de `Payment`
  - Envíos: costo fijo/reglas; captura de dirección y estado básico
- RabbitMQ inicial
  - Publicar `OrderCreated`, `OrderPaid`; worker de notificaciones email
- Mercado Libre (inicial)
  - OAuth app y tokens; job de publicación/actualización simple de ítems
- Frontend
  - Checkout completo y pantallas de estado de pedido/pago
- DevOps
  - Despliegue a entorno de prueba en AWS (EKS) y RDS SQL Server

Entregables: Checkout con pagos operativos, envíos simples, eventos básicos y despliegue de prueba.

### Fase 3 — Post-MVP-2

- Logística y Pagos avanzados
  - Mercado Envíos: cotización, etiquetas, tracking; actualización de `Shipment`
  - Conciliación de pagos (webhooks y reintentos)
- Mercado Libre avanzado
  - Importación de pedidos `MeliOrderImported`, sincronización de stock/precio
- Auditoría ampliada
  - Registro de cambios sensibles (precios, stock, roles) con `DataBefore/DataAfter`
- Mensajería
  - Idempotencia de consumidores, DLQ básica y reintentos exponenciales
- Frontend
  - Vista de órdenes, pagos y envíos enriquecida; panel gestión con reportes básicos

Entregables: Envíos integrados, conciliación de pagos, auditoría robusta, sincronización con Mercado Libre.

### Fase 4 — Post-MVP-3

- Imágenes en Azure Blob Storage (driver concreto tras interfaz)
- Hardening
  - Policies por rol más finas, expiración/refresh tokens, logging estructurado y métricas
- Performance básica
  - Índices adicionales, paginaciones y búsqueda mejorada
- CI/CD
  - Promover a entorno staging/productivo controlado; backups y restauración

Entregables: almacenamiento de imágenes externo, seguridad y operación mejoradas, pipeline maduro.

---

### Módulo vs. entregables por fase

| Módulo | Fase 1 | Fase 2 | Fase 3 | Fase 4 |
|---|---|---|---|---|
| Frontend (Angular 18) | Shell, routing, login | Checkout y estados | Paneles avanzados | Optimización/UI |
| Servicio Tienda (.NET 9) | CRUD catálogo/carrito/pedidos | Pagos y webhooks | Mercado Envíos | Hardening |
| Servicio Gestión (.NET 9) | CRUD admin y auditoría mínima | Reportes básicos | Auditoría ampliada | Backups/operación |
| Integraciones | — | Publicación simple ML | Importación de pedidos, sync stock/precio | — |
| Mensajería (RabbitMQ) | — | Eventos y notificaciones | Idempotencia y DLQ | — |
| DevOps/AWS | Dockerfiles y build | EKS/RDS prueba | Mejora pipelines | Staging/productivo |

### Bibliografía

- Angular 18: https://angular.dev
- Node.js 20 LTS: https://nodejs.org/en
- .NET 9 / ASP.NET Core: https://learn.microsoft.com/aspnet/core
- EF Core 9: https://learn.microsoft.com/ef/core
- SQL Server 2022: https://learn.microsoft.com/sql
- RabbitMQ: https://www.rabbitmq.com/documentation.html
- Docker: https://docs.docker.com
- Helm: https://helm.sh/docs/
- JWT: https://www.rfc-editor.org/rfc/rfc7519
- OAuth 2.0: https://www.rfc-editor.org/rfc/rfc6749
- CORS: https://developer.mozilla.org/docs/Web/HTTP/CORS
- XSS: https://owasp.org/www-community/attacks/xss/
- TLS 1.3: https://www.rfc-editor.org/rfc/rfc8446
## Glosario y Convenciones

- DTO: objeto de transferencia para API; no exponer entidades de dominio.
- Naming: `PascalCase` en código .NET; `camelCase` en JSON; tablas y columnas con convención consistente.
- Tiempos: UTC en base y APIs; conversión en UI según locale.
- Idempotencia: consumidores de colas con deduplicación por `MessageId`.

## Conclusión

Con esta documentación se define el diseño técnico y el plan de ejecución por fases para el MVP de Sastral Commerce, priorizando pagos y envíos, con estándares claros para Angular, .NET 9, EF Core/SQL Server, JWT, RabbitMQ e integraciones (Mercado Pago/Mercado Libre), y una ruta de despliegue en AWS.
