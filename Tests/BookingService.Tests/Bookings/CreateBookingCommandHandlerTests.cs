using BookingService.Application.Bookings.Commands;
using BookingService.Application.Events;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using MassTransit;
using Moq;
using Xunit;

namespace BookingService.Tests.Bookings;

/// <summary>
/// Tests del CreateBookingCommandHandler siguiendo TDD.
/// Cubre: creación exitosa, publicación del evento RabbitMQ y validación de rollback ante error.
/// </summary>
public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly CreateBookingCommandHandler _handler;

    private readonly CreateBookingCommand _validCommand = new(
        HotelId: Guid.NewGuid(),
        RoomId: Guid.NewGuid(),
        GuestFirstName: "Juan",
        GuestLastName: "Pérez",
        GuestDateOfBirth: new DateTime(1990, 5, 15),
        GuestGender: "M",
        GuestDocumentType: "CC",
        GuestDocumentNumber: "123456789",
        GuestEmail: "juan.perez@email.com",
        GuestPhone: "3001234567",
        ArrivalDate: DateTime.UtcNow.AddDays(10),
        DepartureDate: DateTime.UtcNow.AddDays(15),
        NumberOfGuests: 2,
        EmergencyContactName: "María Pérez",
        EmergencyContactPhone: "3009876543"
    );

    public CreateBookingCommandHandlerTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _handler = new CreateBookingCommandHandler(_bookingRepositoryMock.Object, _publishEndpointMock.Object);
    }

    /// <summary>
    /// [GREEN] El handler debe crear la reserva, guardarla y retornar un Guid válido.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateBookingAndReturnGuid()
    {
        // Arrange
        _bookingRepositoryMock.Setup(r => r.AddGuestAsync(It.IsAny<Guest>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _publishEndpointMock.Setup(p => p.Publish(It.IsAny<BookingConfirmedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }

    /// <summary>
    /// [GREEN] Tras crear la reserva, debe publicar exactamente un evento BookingConfirmedEvent.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldPublishBookingConfirmedEvent()
    {
        // Arrange
        _bookingRepositoryMock.Setup(r => r.AddGuestAsync(It.IsAny<Guest>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _publishEndpointMock.Setup(p => p.Publish(It.IsAny<BookingConfirmedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert — exactly one event published
        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<BookingConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// [GREEN] El evento publicado debe contener el email del huésped correcto.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCommand_ShouldPublishEventWithCorrectGuestEmail()
    {
        // Arrange
        BookingConfirmedEvent? capturedEvent = null;

        _bookingRepositoryMock.Setup(r => r.AddGuestAsync(It.IsAny<Guest>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _publishEndpointMock.Setup(p => p.Publish(It.IsAny<BookingConfirmedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<BookingConfirmedEvent, CancellationToken>((evt, _) => capturedEvent = evt)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedEvent);
        Assert.Equal("juan.perez@email.com", capturedEvent!.GuestEmail);
    }

    /// <summary>
    /// [RED→GREEN] Si el repositorio falla al guardar, el evento NO debe publicarse.
    /// </summary>
    [Fact]
    public async Task Handle_WhenSaveFails_ShouldNotPublishEvent()
    {
        // Arrange
        _bookingRepositoryMock.Setup(r => r.AddGuestAsync(It.IsAny<Guest>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Booking>())).Returns(Task.CompletedTask);
        _bookingRepositoryMock.Setup(r => r.SaveChangesAsync())
            .ThrowsAsync(new InvalidOperationException("DB save failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(_validCommand, CancellationToken.None));

        // Verify event was NOT published
        _publishEndpointMock.Verify(
            p => p.Publish(It.IsAny<BookingConfirmedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
