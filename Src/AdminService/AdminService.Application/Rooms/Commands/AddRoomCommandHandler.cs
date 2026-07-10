using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Rooms.Commands;

public class AddRoomCommandHandler : IRequestHandler<AddRoomCommand, Guid>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;

    public AddRoomCommandHandler(IRoomRepository roomRepository, IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<Guid> Handle(AddRoomCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId)
            ?? throw new KeyNotFoundException($"Hotel with Id {request.HotelId} not found.");

        var room = new Room(hotel.Id, request.Type, request.BaseCost, request.Taxes, request.Location);

        await _roomRepository.AddAsync(room);
        await _roomRepository.SaveChangesAsync();

        return room.Id;
    }
}
