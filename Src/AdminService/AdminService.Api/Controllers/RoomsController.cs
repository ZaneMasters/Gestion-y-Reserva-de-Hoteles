using AdminService.Application.Rooms.Commands;
using AdminService.Application.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route("api/hotels/{hotelId}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lista todas las habitaciones de un hotel.</summary>
    [HttpGet]
    public async Task<IActionResult> GetRooms(Guid hotelId)
    {
        var rooms = await _mediator.Send(new GetRoomsByHotelQuery(hotelId));
        return Ok(rooms);
    }

    /// <summary>Agrega una habitación a un hotel.</summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddRoom(Guid hotelId, [FromBody] AddRoomRequest request)
    {
        var command = new AddRoomCommand(hotelId, request.Type, request.BaseCost, request.Taxes, request.Location);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRooms), new { hotelId }, new { RoomId = id });
    }

    /// <summary>Actualiza los datos de una habitación.</summary>
    [HttpPut("{roomId}")]
    [Authorize]
    public async Task<IActionResult> UpdateRoom(Guid hotelId, Guid roomId, [FromBody] UpdateRoomRequest request)
    {
        var command = new UpdateRoomCommand(roomId, request.Type, request.BaseCost, request.Taxes, request.Location);
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>Cambia el estado habilitado/deshabilitado de una habitación.</summary>
    [HttpPatch("{roomId}/status")]
    [Authorize]
    public async Task<IActionResult> ChangeRoomStatus(Guid hotelId, Guid roomId, [FromBody] ChangeStatusRequest request)
    {
        await _mediator.Send(new ChangeRoomStatusCommand(roomId, request.IsEnabled));
        return NoContent();
    }
}

public record AddRoomRequest(string Type, decimal BaseCost, decimal Taxes, string Location);
public record UpdateRoomRequest(string Type, decimal BaseCost, decimal Taxes, string Location);
public record ChangeStatusRequest(bool IsEnabled);
