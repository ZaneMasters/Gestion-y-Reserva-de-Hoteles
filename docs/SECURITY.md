# Documentación de Seguridad — AdminHotels

## 1. Autenticación — JWT (JSON Web Tokens)

### Implementación
Ambos servicios (AdminService y BookingService) usan `Microsoft.AspNetCore.Authentication.JwtBearer`.

- **Algoritmo**: HMAC-SHA256
- **Expiración**: 1 hora
- **Claims incluidos**: `Name`, `Role`, `jti` (ID único del token), `iat` (issued at)

### Obtener un Token
```http
POST /api/auth/login
Content-Type: application/json

{ "username": "admin", "password": "admin123" }
```

### Usar el Token
```http
Authorization: Bearer <token>
```

### Endpoints Protegidos
Solo las operaciones de escritura requieren autenticación:
- `POST /api/hotels` — Crear hotel
- `PUT /api/hotels/{id}` — Actualizar hotel
- `DELETE /api/hotels/{id}` — Eliminar hotel
- `PATCH /api/hotels/{id}/status` — Cambiar estado
- `POST /api/hotels/{id}/rooms` — Agregar habitación
- `PUT /api/hotels/{id}/rooms/{roomId}` — Actualizar habitación
- `PATCH /api/hotels/{id}/rooms/{roomId}/status` — Cambiar estado habitación

Las consultas (GET) son públicas para facilitar la búsqueda de disponibilidad sin registro.

---

## 2. Rate Limiting

### Configuración
Sliding Window: **100 requests por minuto** por cliente.

```csharp
options.AddSlidingWindowLimiter("fixed", limiterOptions => {
    limiterOptions.PermitLimit = 100;
    limiterOptions.Window = TimeSpan.FromMinutes(1);
    limiterOptions.SegmentsPerWindow = 6;
});
```

Respuesta al exceder el límite: **HTTP 429 Too Many Requests**

---

## 3. Validación de Entradas

- Los `Commands` usan DataAnnotations en los campos de entrada.
- Los handlers validan la existencia de entidades antes de operar (`GetByIdAsync` → `KeyNotFoundException`).
- La búsqueda de disponibilidad valida que `ArrivalDate < DepartureDate`.
- Todos los IDs son `Guid`, eliminando inyección por tipo.

---

## 4. Protección contra Inyección

- **SQL Injection**: Entity Framework Core usa parámetros preparados en todas las consultas. Ninguna query es construida con concatenación de strings.
- **NoSQL Injection**: No aplica (se usa PostgreSQL).

---

## 5. Manejo Seguro de Secretos

### En Desarrollo
La clave JWT está en `appsettings.json` (solo para desarrollo local).

### En Producción (recomendado)
Las claves se pasan como **variables de entorno** en `docker-compose.yml`:
```yaml
- Jwt__Key=<secret_desde_vault>
```

En producción real: usar **Azure Key Vault**, **AWS Secrets Manager** o **HashiCorp Vault**.

---

## 6. Logging de Seguridad

El `NotificationService` registra cada evento de reserva confirmada con:
```
[NOTIFICATION] Reserva confirmada. BookingId: {id} | Hotel: {id} | Huésped: {email}
```

En un entorno de producción se integraría con:
- **Serilog** + **Elastic Stack** para centralización de logs
- **OpenTelemetry** para trazas distribuidas
- Alertas sobre intentos de autenticación fallidos

---

## 7. HTTPS

- En producción (detrás de un load balancer): HTTPS termina en el balanceador.
- En desarrollo local: `UseHttpsRedirection()` está habilitado en ambos servicios.
- Para el `docker-compose` local se usa HTTP interno entre contenedores (red privada Docker).

---

## Vulnerabilidades Consideradas y Mitigadas

| Vulnerabilidad | Mitigación |
|---|---|
| SQL Injection | EF Core con parámetros preparados |
| JWT forjado | HMAC-SHA256 con clave secreta de 32+ chars |
| Abuso de API | Rate limiting sliding window |
| Datos sensibles en logs | Solo se loggea email en contexto de confirmación |
| Credentials en código | Variables de entorno en producción |
| CSRF | No aplica (API REST stateless con JWT) |
| Disponibilidad (DoS) | Rate limiting + healthchecks en docker-compose |
