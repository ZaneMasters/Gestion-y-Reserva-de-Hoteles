namespace AdminService.Domain.Interfaces;

using AdminService.Domain.Entities;

public interface IHotelRepository
{
    Task<Hotel?> GetByIdAsync(Guid id);
    Task<IEnumerable<Hotel>> GetAllAsync();
    Task AddAsync(Hotel hotel);
    Task UpdateAsync(Hotel hotel);
    Task DeleteAsync(Hotel hotel); // Logical delete will just update IsEnabled=false
    Task SaveChangesAsync();
}
