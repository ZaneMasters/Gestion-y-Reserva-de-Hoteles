using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Rooms.Commands;

public class ChangeRoomStatusCommandHandler : IRequestHandler<ChangeRoomStatusCommand>
{
    private readonly IRoomRepository _roomRepository;

    public ChangeRoomStatusCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task Handle(ChangeRoomStatusCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId)
            ?? throw new KeyNotFoundException($"Room with Id {request.RoomId} not found.");

        room.ChangeStatus(request.IsEnabled);

        await _roomRepository.UpdateAsync(room);
        await _roomRepository.SaveChangesAsync();
    }
}
