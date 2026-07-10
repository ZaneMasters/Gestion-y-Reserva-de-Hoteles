using MediatR;

namespace AdminService.Application.Hotels.Commands;

public record CreateHotelCommand(
    string Name,
    string City,
    string Address,
    string Description
) : IRequest<Guid>;
