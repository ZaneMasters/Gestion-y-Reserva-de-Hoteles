namespace BookingService.Application.Bookings.Queries;

/// <summary>
/// DTO que representa una habitación disponible retornada por el AdminService
/// y verificada contra reservas existentes.
/// </summary>
public class AvailableRoomDto
{
    public Guid RoomId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public decimal BaseCost { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalCost => BaseCost + Taxes;
    public string Location { get; set; } = string.Empty;
}

/// <summary>
/// DTO interno para deserializar la respuesta del AdminService.
/// </summary>
public class HotelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public List<RoomDto> Rooms { get; set; } = new();
}

public class RoomDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal BaseCost { get; set; }
    public decimal Taxes { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}
