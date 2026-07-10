using BookingService.Domain.Entities;
using MediatR;

namespace BookingService.Application.Bookings.Queries;

public record GetBookingByIdQuery(Guid BookingId) : IRequest<Booking?>;
