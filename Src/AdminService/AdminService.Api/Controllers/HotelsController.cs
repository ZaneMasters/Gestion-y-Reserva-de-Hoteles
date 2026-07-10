using AdminService.Application.Hotels.Commands;
using AdminService.Application.Hotels.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lista todos los hoteles.</summary>
    [HttpGet]
    public async Task<IActionResult> GetHotels()
    {
        var hotels = await _mediator.Send(new GetHotelsQuery());
        return Ok(hotels);
    }

    /// <summary>Obtiene el detalle de un hotel por ID.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotelById(Guid id)
    {
        var hotel = await _mediator.Send(new GetHotelByIdQuery(id));
        if (hotel is null) return NotFound();
        return Ok(hotel);
    }

    /// <summary>Crea un nuevo hotel.</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotelById), new { id }, new { HotelId = id });
    }

    /// <summary>Actualiza los datos de un hotel.</summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelRequest request)
    {
        var command = new UpdateHotelCommand(id, request.Name, request.City, request.Address, request.Description);
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>Eliminación lógica de un hotel (lo deshabilita).</summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _mediator.Send(new DeleteHotelCommand(id));
        return NoContent();
    }

    /// <summary>Habilita o deshabilita un hotel.</summary>
    [HttpPatch("{id}/status")]
    [Authorize]
    public async Task<IActionResult> ChangeHotelStatus(Guid id, [FromBody] ChangeHotelStatusRequest request)
    {
        await _mediator.Send(new ChangeHotelStatusCommand(id, request.IsEnabled));
        return NoContent();
    }
}

public record UpdateHotelRequest(string Name, string City, string Address, string Description);
public record ChangeHotelStatusRequest(bool IsEnabled);
