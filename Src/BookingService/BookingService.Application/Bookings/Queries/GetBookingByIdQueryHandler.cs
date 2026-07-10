using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Bookings.Queries;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Booking?>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetByIdWithGuestAsync(request.BookingId);
    }
}
