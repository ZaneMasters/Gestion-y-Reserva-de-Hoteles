using AdminService.Domain.Entities;

namespace AdminService.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id);
    Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId);
    Task AddAsync(Room room);
    Task UpdateAsync(Room room);
    Task SaveChangesAsync();
}
