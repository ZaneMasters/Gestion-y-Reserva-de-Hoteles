using MediatR;

namespace AdminService.Application.Hotels.Commands;

public record ChangeHotelStatusCommand(Guid HotelId, bool IsEnabled) : IRequest;
