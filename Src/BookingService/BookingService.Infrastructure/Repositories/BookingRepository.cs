using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking?> GetByIdWithGuestAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Guest)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetByHotelIdAsync(Guid hotelId)
    {
        return await _context.Bookings
            .Include(b => b.Guest)
            .Where(b => b.HotelId == hotelId)
            .OrderByDescending(b => b.ArrivalDate)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene reservas que se solapan con el rango de fechas dado para una habitación.
    /// Condición de solapamiento: arrival1 < departure2 AND departure1 > arrival2
    /// </summary>
    public async Task<IEnumerable<Booking>> GetOverlappingBookingsAsync(Guid roomId, DateTime arrival, DateTime departure)
    {
        return await _context.Bookings
            .Where(b => b.RoomId == roomId
                && b.Status == BookingStatus.Confirmed
                && b.ArrivalDate < departure
                && b.DepartureDate > arrival)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await Task.CompletedTask;
    }

    public async Task AddGuestAsync(Guest guest)
    {
        await _context.Guests.AddAsync(guest);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
