# Uso de Inteligencia Artificial — AdminHotels

## Herramienta Utilizada

**Antigravity (Google DeepMind)** — Asistente de coding IA integrado en el IDE.

---

## ¿Para qué se utilizó?

| Área | Uso de IA |
|---|---|
| **Scaffolding inicial** | Generación de la estructura base de Clean Architecture por microservicio |
| **CQRS boilerplate** | Commands, Queries y Handlers repetitivos (patrón consistente) |
| **Configuración MassTransit** | Setup de RabbitMQ con retry policies y dead-letter queues |
| **Tests TDD** | Generación de casos de prueba con Moq siguiendo el patrón Arrange/Act/Assert |
| **Documentación** | README, ADR, SECURITY.md y AI_USAGE.md |
| **docker-compose** | Healthchecks, variables de entorno y dependencias entre servicios |

---

## ¿Cómo se validó el código generado?

1. **Revisión manual de cada archivo**: Se verificó la coherencia entre interfaces, implementaciones y registros DI antes de aceptar cada cambio.
2. **Compilación**: Se verificó que el proyecto compila sin errores con `dotnet build`.
3. **Revisión de lógica de negocio**: La lógica de solapamiento de reservas (`GetOverlappingBookingsAsync`) fue revisada matemáticamente: `arrival1 < departure2 AND departure1 > arrival2`.
4. **Revisión de seguridad**: Se verificó que el orden de middlewares en `Program.cs` es correcto: `UseAuthentication()` antes de `UseAuthorization()`.
5. **Tests unitarios**: Los tests verifican el comportamiento esperado de los handlers de forma independiente de la infraestructura.

---

## Ejemplo Concreto: Generación del SearchAvailableRoomsQueryHandler

### Prompt Utilizado

> "Implementa el handler `SearchAvailableRoomsQueryHandler` para MediatR en C#. Este handler debe:
> 1. Obtener todos los hoteles habilitados del AdminService via HttpClient (GET /api/hotels)
> 2. Filtrar por ciudad (case-insensitive)
> 3. Para cada habitación habilitada en esos hoteles, verificar que no haya reservas solapadas en el rango de fechas dado (usando IBookingRepository.GetOverlappingBookingsAsync)
> 4. Retornar una lista de AvailableRoomDto con: RoomId, HotelId, HotelName, City, RoomType, BaseCost, Taxes, TotalCost (computed), Location
> La clase debe usar IHttpClientFactory con un named client 'AdminService' y IConfiguration para leer la URL base."

### Resultado y Validaciones

El código generado implementó correctamente:
- ✅ Inyección de `IHttpClientFactory` y `IBookingRepository`
- ✅ Filtrado case-insensitive con `StringComparison.OrdinalIgnoreCase`
- ✅ Verificación de disponibilidad habitación por habitación
- ✅ Propiedad calculada `TotalCost = BaseCost + Taxes`

**Ajuste manual realizado**: Se agregó el manejo del caso donde `GetFromJsonAsync` retorna null, usando el operador `??` con `Enumerable.Empty<HotelDto>()` para evitar NullReferenceException si el AdminService retorna respuesta vacía.

---

## Reflexión sobre el Uso de IA

La IA fue especialmente útil para **reducir el tiempo en boilerplate** (Commands, Handlers, DTOs repetitivos) y para **documentación**. Sin embargo, fue necesaria supervisión humana en:

- **Orden de configuración en Program.cs**: La IA inicialmente omitió `UseAuthentication()` antes de `UseAuthorization()`.
- **Lógica de solapamiento de fechas**: Se verificó matemáticamente la condición del intervalo.
- **Configuración de retry en MassTransit**: Se ajustaron los parámetros de backoff exponencial para ser razonables (2s inicial, máx 30s).

El enfoque adoptado fue **IA como copiloto, humano como piloto**: la IA propone, el desarrollador revisa, ajusta y valida.
