using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;
using MediatR;

namespace AdminService.Application.Rooms.Queries;

public class GetRoomsByHotelQueryHandler : IRequestHandler<GetRoomsByHotelQuery, IEnumerable<Room>>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomsByHotelQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<Room>> Handle(GetRoomsByHotelQuery request, CancellationToken cancellationToken)
    {
        return await _roomRepository.GetByHotelIdAsync(request.HotelId);
    }
}
