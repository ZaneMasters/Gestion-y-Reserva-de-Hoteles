namespace AdminService.Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public decimal BaseCost { get; private set; }
    public decimal Taxes { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public bool IsEnabled { get; private set; }

    public Hotel Hotel { get; private set; } = null!;

    // For EF Core
    protected Room() { }

    public Room(Guid hotelId, string type, decimal baseCost, decimal taxes, string location)
    {
        Id = Guid.NewGuid();
        HotelId = hotelId;
        Type = type;
        BaseCost = baseCost;
        Taxes = taxes;
        Location = location;
        IsEnabled = true; // Enabled by default
    }

    public void Update(string type, decimal baseCost, decimal taxes, string location)
    {
        Type = type;
        BaseCost = baseCost;
        Taxes = taxes;
        Location = location;
    }

    public void ChangeStatus(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
