using MediatR;

namespace AdminService.Application.Rooms.Commands;

public record AddRoomCommand(
    Guid HotelId,
    string Type,
    decimal BaseCost,
    decimal Taxes,
    string Location
) : IRequest<Guid>;
