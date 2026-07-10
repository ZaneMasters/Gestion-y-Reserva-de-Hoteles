using BookingService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace BookingService.Application.Bookings.Queries;

/// <summary>
/// Búsqueda de disponibilidad en dos pasos:
/// 1. Obtiene hoteles y habitaciones habilitadas del AdminService por HTTP.
/// 2. Filtra por ciudad, y verifica que no haya reservas solapadas en el BookingService local.
/// </summary>
public class SearchAvailableRoomsQueryHandler : IRequestHandler<SearchAvailableRoomsQuery, IEnumerable<AvailableRoomDto>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public SearchAvailableRoomsQueryHandler(
        IBookingRepository bookingRepository,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _bookingRepository = bookingRepository;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IEnumerable<AvailableRoomDto>> Handle(SearchAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Get all enabled hotels with their enabled rooms from AdminService
        var client = _httpClientFactory.CreateClient("AdminService");
        var hotels = await client.GetFromJsonAsync<IEnumerable<HotelDto>>("api/hotels", cancellationToken)
                     ?? Enumerable.Empty<HotelDto>();

        // Step 2: Filter by city (case-insensitive) and enabled status
        var hotelsInCity = hotels
            .Where(h => h.IsEnabled && h.City.Equals(request.City, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var availableRooms = new List<AvailableRoomDto>();

        foreach (var hotel in hotelsInCity)
        {
            foreach (var room in hotel.Rooms.Where(r => r.IsEnabled))
            {
                // Step 3: Check for overlapping bookings in our own DB
                var overlaps = await _bookingRepository.GetOverlappingBookingsAsync(
                    room.Id, request.ArrivalDate, request.DepartureDate);

                if (!overlaps.Any())
                {
                    availableRooms.Add(new AvailableRoomDto
                    {
                        RoomId = room.Id,
                        HotelId = hotel.Id,
                        HotelName = hotel.Name,
                        City = hotel.City,
                        RoomType = room.Type,
                        BaseCost = room.BaseCost,
                        Taxes = room.Taxes,
                        Location = room.Location
                    });
                }
            }
        }

        return availableRooms;
    }
}
