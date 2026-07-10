using MediatR;

namespace AdminService.Application.Hotels.Commands;

public record UpdateHotelCommand(
    Guid HotelId,
    string Name,
    string City,
    string Address,
    string Description
) : IRequest;
