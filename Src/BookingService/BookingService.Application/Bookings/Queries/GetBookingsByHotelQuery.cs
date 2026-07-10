using BookingService.Domain.Entities;
using MediatR;

namespace BookingService.Application.Bookings.Queries;

public record GetBookingsByHotelQuery(Guid HotelId) : IRequest<IEnumerable<Booking>>;
