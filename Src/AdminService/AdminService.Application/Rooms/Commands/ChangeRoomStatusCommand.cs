using MediatR;

namespace AdminService.Application.Rooms.Commands;

public record ChangeRoomStatusCommand(Guid RoomId, bool IsEnabled) : IRequest;
