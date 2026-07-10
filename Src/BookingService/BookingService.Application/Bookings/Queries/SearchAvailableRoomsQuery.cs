using MediatR;

namespace BookingService.Application.Bookings.Queries;

public record SearchAvailableRoomsQuery(
    string City,
    DateTime ArrivalDate,
    DateTime DepartureDate,
    int NumberOfGuests
) : IRequest<IEnumerable<AvailableRoomDto>>;
