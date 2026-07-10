# AdminHotels — Sistema de Gestión y Reserva de Hoteles

Backend para una agencia de viajes que permite gestionar hoteles, habitaciones y reservas. Desarrollado como prueba técnica con arquitectura de microservicios.

## Arquitectura

```
┌─────────────┐     HTTP      ┌─────────────────┐    PostgreSQL
│  API Gateway│──────────────▶│  AdminService   │──────────────▶ admin_db
│   (YARP)    │               │  Port 8081      │
│  Port 8080  │     HTTP      └─────────────────┘
│             │──────────────▶┌─────────────────┐    PostgreSQL
└─────────────┘               │ BookingService  │──────────────▶ booking_db
                              │  Port 8082      │
                              └────────┬────────┘
                                       │ Publish Event
                                       ▼
                              ┌─────────────────┐
                              │    RabbitMQ     │
                              │  Port 5672      │
                              └────────┬────────┘
                                       │ Consume Event
                                       ▼
                              ┌─────────────────┐
                              │Notification Svc │
                              │  (Worker)       │
                              └─────────────────┘
```

### Bounded Contexts (DDD)
- **AdminContext**: Gestión de hoteles y habitaciones (entidades propias: `Hotel`, `Room`)
- **BookingContext**: Reservas y huéspedes (entidades propias: `Booking`, `Guest`)
- **NotificationContext**: Envío de notificaciones (reacciona a eventos del BookingContext)

## Retos Técnicos Implementados

| Reto | Implementación |
|---|---|
| **Microservicios** | AdminService + BookingService + NotificationService + ApiGateway |
| **RabbitMQ** | MassTransit: publica `BookingConfirmedEvent`, retry exponencial (3 intentos) |
| **DDD** | Bounded Contexts, Entities, Aggregates, Domain Events |
| **CQRS** | MediatR: Commands (escritura) y Queries (lectura) separados |
| **JWT** | Bearer tokens con expiración 1h, válidos en ambos servicios |
| **Rate Limiting** | Sliding window: 100 req/min por endpoint |
| **Validación** | DataAnnotations + validación de negocio en handlers |
| **TDD** | 8 tests unitarios con xUnit + Moq (Red/Green/Refactor documentado) |
| **Clean Architecture** | Api → Application → Domain ← Infrastructure |
| **Docker** | Builds multi-stage, healthchecks, docker-compose completo |

## Requisitos de Ejecución

Solo necesitas:
- **Docker** y **Docker Compose**

## Levantar el Proyecto

```bash
# Clonar y ejecutar
git clone <repo-url>
cd AdminHotels
docker-compose up --build
```

## Endpoints Disponibles

### Vía API Gateway (http://localhost:8080)

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| `POST` | `/api/auth/login` | ❌ | Obtener JWT |
| `GET` | `/api/hotels` | ❌ | Listar hoteles |
| `GET` | `/api/hotels/{id}` | ❌ | Detalle de hotel |
| `POST` | `/api/hotels` | ✅ | Crear hotel |
| `PUT` | `/api/hotels/{id}` | ✅ | Actualizar hotel |
| `DELETE` | `/api/hotels/{id}` | ✅ | Eliminar hotel (lógico) |
| `PATCH` | `/api/hotels/{id}/status` | ✅ | Habilitar/deshabilitar hotel |
| `GET` | `/api/hotels/{id}/bookings` | ✅ | Reservas de un hotel |
| `GET` | `/api/hotels/{hotelId}/rooms` | ❌ | Habitaciones de un hotel |
| `POST` | `/api/hotels/{hotelId}/rooms` | ✅ | Agregar habitación |
| `PUT` | `/api/hotels/{hotelId}/rooms/{roomId}` | ✅ | Actualizar habitación |
| `PATCH` | `/api/hotels/{hotelId}/rooms/{roomId}/status` | ✅ | Habilitar/deshabilitar habitación |
| `GET` | `/api/bookings/available?city=&arrival=&departure=&guests=` | ❌ | Buscar habitaciones disponibles |
| `POST` | `/api/bookings` | ❌ | Crear reserva |
| `GET` | `/api/bookings/{id}` | ✅ | Detalle de reserva |
| `GET` | `/api/bookings/hotel/{hotelId}` | ✅ | Reservas de un hotel |

### Credenciales Demo JWT
```json
POST /api/auth/login
{ "username": "admin", "password": "admin123" }
{ "username": "agent", "password": "agent123" }
```

### Consolas de Administración
- **AdminService Swagger**: http://localhost:8081/swagger
- **BookingService Swagger**: http://localhost:8082/swagger
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)

## Decisiones Técnicas

### ¿Por qué PostgreSQL?
Base de datos relacional con soporte nativo para UUID, JSON y transacciones ACID. Ideal para datos de reservas donde la consistencia es crítica. Trade-off: menos flexible que NoSQL para esquemas dinámicos, pero las reservas hoteleras tienen esquemas bien definidos.

### ¿Por qué MassTransit sobre RabbitMQ nativo?
MassTransit agrega retry policies, dead-letter queues, serialización automática y abstracción del broker. Permite cambiar de RabbitMQ a Azure Service Bus o Amazon SQS sin cambiar el código de negocio.

### ¿Por qué YARP como API Gateway?
YARP (Yet Another Reverse Proxy) es el proxy recomendado por Microsoft para .NET, con configuración declarativa en JSON y soporte nativo para transformaciones de headers, load balancing y health checks.

### ¿Por qué CQRS con MediatR?
Separa la intención de lectura (Query) y escritura (Command). Facilita escalar el stack de lectura independientemente, agregar caché a las queries y aplicar políticas diferentes (validación, retry) por tipo de operación.

## Correr los Tests

```bash
dotnet test Tests/AdminService.Tests/AdminService.Tests.csproj
dotnet test Tests/BookingService.Tests/BookingService.Tests.csproj

# O todos a la vez
dotnet test AdminHotels.slnx
```

## Documentación Adicional

- [Decisiones de Arquitectura (ADR)](docs/ADR.md)
- [Seguridad](docs/SECURITY.md)
- [Uso de IA](docs/AI_USAGE.md)
