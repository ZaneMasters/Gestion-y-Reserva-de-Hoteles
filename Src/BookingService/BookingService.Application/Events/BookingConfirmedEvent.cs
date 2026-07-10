namespace BookingService.Application.Events;

public record BookingConfirmedEvent(
    Guid BookingId,
    Guid HotelId,
    Guid RoomId,
    string GuestEmail,
    DateTime ArrivalDate
);
