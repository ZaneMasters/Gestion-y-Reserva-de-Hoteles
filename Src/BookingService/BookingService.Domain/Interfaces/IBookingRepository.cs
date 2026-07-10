using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);
    Task<Booking?> GetByIdWithGuestAsync(Guid id);
    Task<IEnumerable<Booking>> GetByHotelIdAsync(Guid hotelId);
    Task<IEnumerable<Booking>> GetOverlappingBookingsAsync(Guid roomId, DateTime arrival, DateTime departure);
    Task AddAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task AddGuestAsync(Guest guest);
    Task SaveChangesAsync();
}

