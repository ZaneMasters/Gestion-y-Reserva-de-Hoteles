using BookingService.Application.Events;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using MassTransit;
using MediatR;

namespace BookingService.Application.Bookings.Commands;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IPublishEndpoint publishEndpoint)
    {
        _bookingRepository = bookingRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // 1. Register Guest
        var guest = new Guest(request.GuestFirstName, request.GuestLastName, request.GuestDateOfBirth.ToUniversalTime(),
            request.GuestGender, request.GuestDocumentType, request.GuestDocumentNumber,
            request.GuestEmail, request.GuestPhone);
        
        await _bookingRepository.AddGuestAsync(guest);

        // 2. Create Booking
        var booking = new Booking(request.HotelId, request.RoomId, guest.Id, request.ArrivalDate.ToUniversalTime(),
            request.DepartureDate.ToUniversalTime(), request.NumberOfGuests, request.EmergencyContactName,
            request.EmergencyContactPhone);

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        // 3. Publish Event to RabbitMQ
        var bookingConfirmedEvent = new BookingConfirmedEvent(booking.Id, booking.HotelId, booking.RoomId, guest.Email, booking.ArrivalDate);
        await _publishEndpoint.Publish(bookingConfirmedEvent, cancellationToken);

        return booking.Id;
    }
}
