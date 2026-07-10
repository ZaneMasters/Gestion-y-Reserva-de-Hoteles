# UltraGroup – Tech, Travel & Loyalty

# Prueba Técnica

## Back End Developer

### Sistema de Gestión y Reserva de Hoteles

**Versión:** 2.0 | 2025

**Duración estimada:** 4 a 5 días hábiles

**Niveles evaluados:**

- Junior
- Semi Senior
- Senior
- Lead

---

# 1. Contexto y objetivo

Una agencia de viajes necesita modernizar su plataforma de alojamiento.

Actualmente administra hoteles y reservas de forma manual, lo que genera inconsistencias, pérdida de comisiones y mala experiencia para los viajeros.

El objetivo es diseñar un backend que sea:

- Robusto
- Escalable
- Mantenible

La funcionalidad base puede ser realizada por cualquier nivel, mientras que los retos avanzados sirven para demostrar un mayor nivel técnico.

---

# 2. Requisitos funcionales

## Historia de usuario 1

### Administración de hoteles

Como agente de viajes quiero gestionar hoteles y habitaciones.

El sistema debe permitir:

- Crear hotel
- Editar hotel
- Eliminar hotel (lógico)

Cada hotel debe registrar como mínimo:

- Nombre
- Ciudad
- Dirección
- Descripción
- Estado (habilitado/deshabilitado)

Cada habitación debe registrar:

- Tipo
- Costo base
- Impuestos
- Ubicación dentro del hotel
- Estado

También debe permitir:

- Modificar hoteles
- Modificar habitaciones
- Habilitar/deshabilitar hoteles
- Habilitar/deshabilitar habitaciones
- Listar reservas de un hotel
- Consultar detalle de una reserva

---

## Historia de usuario 2

### Reserva de habitaciones

Como viajero quiero buscar habitaciones disponibles.

La búsqueda debe permitir filtrar por:

- Ciudad destino
- Fecha de entrada
- Fecha de salida
- Disponibilidad

(El OCR no logró recuperar completamente el resto del texto.)

---

# 3. Retos técnicos diferenciadores

Estos retos no son obligatorios.

Sirven para demostrar un mayor nivel técnico.

## Arquitectura de microservicios

Separar al menos:

- Gestión de hoteles
- Gestión de reservas

Documentar contratos OpenAPI.

---

## RabbitMQ

Publicar eventos como:

ReservaConfirmada

Consumidos por un servicio de notificaciones.

Explicar:

- Reintentos
- Manejo de fallos

---

## Seguridad

Implementar:

- JWT
- OAuth2 (o equivalente)

Además aplicar al menos tres prácticas como:

- Validación de entradas
- Protección contra inyección
- Rate limiting
- Manejo seguro de secretos
- HTTPS
- Logging de seguridad

---

## Uso de IA

Documentar el uso de herramientas como:

- GitHub Copilot
- Cursor
- Codeium
- Tabnine

Explicar:

- Para qué se utilizó
- Cómo se validó el código generado

Mostrar al menos un ejemplo donde la IA ayudó a generar o refactorizar un módulo.

Incluir el prompt utilizado.

---

## TDD

Implementar al menos un flujo crítico usando:

- Red
- Green
- Refactor

Mostrar commits asociados.

---

## DDD

Definir al menos:

- Bounded Contexts
- Entities
- Value Objects
- Aggregates
- Domain Events

---

## Telemetría

Agregar observabilidad.

---

## Clean Code

Aplicar buenas prácticas y justificar decisiones de diseño.

---

# Stack técnico

## Lenguaje

Se requiere:

**C# con .NET 10**

Si se utiliza otra versión debe justificarse técnicamente.

---

## Persistencia

Puede utilizarse:

- SQL
- NoSQL

Debe justificarse:

- Motivo de la elección
- Trade-offs
- Cuándo sería conveniente cambiar de tecnología

---

## API

Puede implementarse con:

- REST
- gRPC

Si se usa REST:

- Swagger
- OpenAPI

Si existen múltiples servicios:

- API Gateway
- o documentar la orquestación.

---

# Entregables

Repositorio público.

README que incluya:

- Instrucciones de ejecución
- Arquitectura
- Decisiones técnicas
- ADR
- Diagramas C4

Además:

- Documentación de seguridad
- Documentación del uso de IA
- Colección Postman
- docker-compose funcional (si aplica)

---

# Preguntas para la sustentación

Debes poder responder preguntas como:

- ¿Por qué elegiste esa arquitectura?
- ¿Cómo escalarías a 10.000 reservas diarias?
- ¿Cómo rediseñarías el modelo de datos?
- ¿Cómo evitarías pérdida o duplicación de mensajes RabbitMQ?
- ¿Dónde definirías los Bounded Context?
- ¿Cómo monitorearías la latencia?
- ¿Cómo versionarías la API?
- ¿Qué vulnerabilidades de seguridad consideraste?