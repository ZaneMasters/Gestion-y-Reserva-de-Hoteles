using AdminService.Domain.Entities;
using MediatR;

namespace AdminService.Application.Rooms.Queries;

public record GetRoomsByHotelQuery(Guid HotelId) : IRequest<IEnumerable<Room>>;
