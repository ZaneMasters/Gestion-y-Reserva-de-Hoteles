using MediatR;

namespace BookingService.Application.Bookings.Commands;

public record CreateBookingCommand(
    Guid HotelId,
    Guid RoomId,
    string GuestFirstName,
    string GuestLastName,
    DateTime GuestDateOfBirth,
    string GuestGender,
    string GuestDocumentType,
    string GuestDocumentNumber,
    string GuestEmail,
    string GuestPhone,
    DateTime ArrivalDate,
    DateTime DepartureDate,
    int NumberOfGuests,
    string EmergencyContactName,
    string EmergencyContactPhone
) : IRequest<Guid>;
