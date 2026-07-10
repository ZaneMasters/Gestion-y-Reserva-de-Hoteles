using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Bookings.Queries;

public class GetBookingsByHotelQueryHandler : IRequestHandler<GetBookingsByHotelQuery, IEnumerable<Booking>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsByHotelQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<Booking>> Handle(GetBookingsByHotelQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetByHotelIdAsync(request.HotelId);
    }
}
