using MediatR;

namespace AdminService.Application.Rooms.Commands;

public record UpdateRoomCommand(
    Guid RoomId,
    string Type,
    decimal BaseCost,
    decimal Taxes,
    string Location
) : IRequest;
