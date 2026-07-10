using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Rooms.Commands;

public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;

    public UpdateRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId)
            ?? throw new KeyNotFoundException($"Room with Id {request.RoomId} not found.");

        room.Update(request.Type, request.BaseCost, request.Taxes, request.Location);

        await _roomRepository.UpdateAsync(room);
        await _roomRepository.SaveChangesAsync();
    }
}
