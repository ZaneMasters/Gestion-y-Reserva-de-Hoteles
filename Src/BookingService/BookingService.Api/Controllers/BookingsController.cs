using BookingService.Application.Bookings.Commands;
using BookingService.Application.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookingService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Busca habitaciones disponibles filtradas por ciudad, fechas y cantidad de huéspedes.
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> SearchAvailable(
        [FromQuery] string city,
        [FromQuery] DateTime arrival,
        [FromQuery] DateTime departure,
        [FromQuery] int guests = 1)
    {
        if (arrival >= departure)
            return BadRequest("La fecha de llegada debe ser anterior a la fecha de salida.");

        var query = new SearchAvailableRoomsQuery(city, arrival, departure, guests);
        var results = await _mediator.Send(query);
        return Ok(results);
    }

    /// <summary>Obtiene el detalle de una reserva específica.</summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery(id));
        if (booking is null) return NotFound();
        return Ok(booking);
    }

    /// <summary>Lista todas las reservas de un hotel.</summary>
    [HttpGet("hotel/{hotelId}")]
    [Authorize]
    public async Task<IActionResult> GetBookingsByHotel(Guid hotelId)
    {
        var bookings = await _mediator.Send(new GetBookingsByHotelQuery(hotelId));
        return Ok(bookings);
    }

    /// <summary>Crea una nueva reserva y publica el evento de confirmación.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { BookingId = id, Message = "Reserva creada. El evento de confirmación fue enviado." });
    }
}
