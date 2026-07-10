using MassTransit;
using NotificationService.Events;

namespace NotificationService.Consumers;

/// <summary>
/// Consumidor del evento BookingConfirmedEvent.
/// En un sistema real, aquí se invocaría un servicio de email (SendGrid, SMTP, etc.).
/// </summary>
public class BookingConfirmedConsumer : IConsumer<BookingConfirmedEvent>
{
    private readonly ILogger<BookingConfirmedConsumer> _logger;

    public BookingConfirmedConsumer(ILogger<BookingConfirmedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BookingConfirmedEvent> context)
    {
        var evt = context.Message;

        _logger.LogInformation(
            "[NOTIFICATION] Reserva confirmada. BookingId: {BookingId} | Hotel: {HotelId} | " +
            "Habitación: {RoomId} | Huésped: {GuestEmail} | Llegada: {ArrivalDate:yyyy-MM-dd}",
            evt.BookingId, evt.HotelId, evt.RoomId, evt.GuestEmail, evt.ArrivalDate);

        // Simulate email sending
        await SimulateSendEmailAsync(evt);

        _logger.LogInformation(
            "[NOTIFICATION] Email de confirmación enviado a {GuestEmail} para la reserva {BookingId}.",
            evt.GuestEmail, evt.BookingId);
    }

    private async Task SimulateSendEmailAsync(BookingConfirmedEvent evt)
    {
        // In production: integrate with SendGrid / AWS SES / SMTP
        await Task.Delay(100); // Simulate async email sending
    }
}
