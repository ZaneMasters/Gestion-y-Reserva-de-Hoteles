# Architecture Decision Records (ADR) — AdminHotels

## ADR-001: Arquitectura de Microservicios

**Estado**: Aceptado

**Contexto**: Una agencia de viajes necesita modernizar su sistema de gestión de hoteles y reservas. El sistema debe ser escalable y mantenible.

**Decisión**: Separar la solución en microservicios independientes:
- `AdminService`: Gestión de hoteles y habitaciones
- `BookingService`: Búsqueda de disponibilidad y reservas
- `NotificationService`: Envío de notificaciones por eventos

**Consecuencias**:
- ✅ Cada servicio escala independientemente
- ✅ Despliegue independiente sin afectar otros servicios
- ✅ Fallos aislados (si cae un servicio, los demás siguen operando)
- ⚠️ Mayor complejidad operacional (múltiples DBs, networking, Docker)
- ⚠️ Latencia adicional por comunicación HTTP entre servicios

---

## ADR-002: PostgreSQL como Motor de Base de Datos

**Estado**: Aceptado

**Contexto**: Se necesita persistencia para hoteles, habitaciones, reservas y huéspedes.

**Decisión**: PostgreSQL con Entity Framework Core y bases de datos separadas por servicio.

**Justificación**:
- Datos relacionales con integridad referencial (hotel → habitaciones, reserva → huésped)
- Soporte nativo para UUID, tipos JSON, full-text search
- Open source, maduro y con excelente soporte en .NET

**Trade-offs**:
- ✅ ACID, transacciones, joins complejos
- ✅ Sin costo de licencia
- ⚠️ Escala verticalmente por defecto; para escala horizontal se requiere sharding o read replicas

**¿Cuándo cambiar?**: Si el volumen de reservas supera 10 millones diarias, considerar particionamiento o Citus. Para datos de disponibilidad en tiempo real con alta concurrencia, complementar con Redis.

---

## ADR-003: MassTransit + RabbitMQ para Mensajería Asíncrona

**Estado**: Aceptado

**Contexto**: El `BookingService` necesita notificar al `NotificationService` cuando se confirma una reserva, sin acoplamiento directo.

**Decisión**: MassTransit con RabbitMQ como broker.

**Justificación**:
- Desacoplamiento: el BookingService no conoce al NotificationService
- Retry automático con backoff exponencial (3 intentos: 2s, 4s, 8s)
- Dead-letter queue automático para mensajes que fallan repetidamente
- MassTransit permite cambiar de broker sin cambiar el código de negocio

**Manejo de Fallos**:
- Retry exponencial antes de mover a dead-letter queue
- Los mensajes en la DLQ pueden ser inspeccionados y reprocesados desde el panel de RabbitMQ Management

**¿Cómo evitar pérdida o duplicación?**:
- RabbitMQ garantiza entrega at-least-once
- Los consumers de MassTransit son idempotentes por diseño (loggean el `BookingId` único)
- Para idempotencia estricta en producción: almacenar IDs de eventos procesados en Redis

---

## ADR-004: CQRS con MediatR

**Estado**: Aceptado

**Contexto**: Los casos de uso de lectura y escritura tienen características diferentes (caché, validaciones, proyecciones).

**Decisión**: Patrón CQRS implementado con MediatR.

**Consecuencias**:
- ✅ Commands y Queries con handlers separados y focused
- ✅ Fácil agregar behaviors (logging, validación, retry) via MediatR pipeline
- ✅ Las Queries pueden optimizarse con proyecciones sin afectar los Commands
- ⚠️ Más archivos por caso de uso (Command + Handler por operación)

---

## ADR-005: YARP como API Gateway

**Estado**: Aceptado

**Contexto**: Se necesita un punto de entrada único para los microservicios.

**Decisión**: YARP (Yet Another Reverse Proxy) en un proyecto ASP.NET Core separado.

**Justificación**:
- Mantenido por Microsoft, integración nativa con .NET
- Configuración declarativa en JSON
- Soporte para load balancing, transformaciones, health checks

**¿Cuándo escalar?**: Para producción se complementaría con Kong, Nginx o Azure API Management para agregar autenticación a nivel gateway, throttling centralizado y observabilidad.

---

## ADR-006: Clean Architecture por Microservicio

**Estado**: Aceptado

**Contexto**: Cada microservicio necesita ser mantenible y testeable.

**Decisión**: Cada servicio sigue Clean Architecture con 4 capas:
- `Domain`: Entidades, interfaces, value objects (sin dependencias externas)
- `Application`: Casos de uso, Commands, Queries (solo depende de Domain)
- `Infrastructure`: EF Core, repositorios, MassTransit (implementa interfaces del Domain)
- `Api`: Controllers, DTOs, configuración (orquesta Application e Infrastructure)

**Consecuencias**:
- ✅ El Domain es testeable sin infraestructura (se mockea el repositorio)
- ✅ Cambiar PostgreSQL por otro motor solo requiere cambiar Infrastructure
- ⚠️ Más proyectos y archivos por microservicio

---

## ADR-007: Comunicación Síncrona entre Servicios vía HTTP

**Estado**: Aceptado

**Contexto**: El `BookingService` necesita obtener la lista de hoteles y habitaciones habilitadas del `AdminService` para verificar disponibilidad.

**Decisión**: HttpClient tipado desde BookingService hacia AdminService.

**Justificación**:
- Simplicidad para el caso de búsqueda (requiere datos actualizados en el momento)
- No introduce un broker adicional para una consulta de lectura

**Trade-offs**:
- ⚠️ Acoplamiento temporal: si AdminService cae, la búsqueda de disponibilidad falla
- ⚠️ Latencia adicional por llamada HTTP

**Alternativas para escalar**:
1. **Cache local**: El BookingService mantiene una copia local sincronizada via eventos (CQRS Read Model)
2. **Circuit Breaker**: Polly para reintentos y fallback si AdminService no responde
