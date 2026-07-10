namespace NotificationService.Events;

/// <summary>
/// Contrato del evento compartido con BookingService.
/// Debe ser idéntico al publicado para que MassTransit lo deserialice correctamente.
/// </summary>
public record BookingConfirmedEvent(
    Guid BookingId,
    Guid HotelId,
    Guid RoomId,
    string GuestEmail,
    DateTime ArrivalDate
);
