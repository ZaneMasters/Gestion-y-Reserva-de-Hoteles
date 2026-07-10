namespace AdminService.Domain.Entities;

public class Hotel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsEnabled { get; private set; }
    
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    // For EF Core
    protected Hotel() { }

    public Hotel(string name, string city, string address, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        City = city;
        Address = address;
        Description = description;
        IsEnabled = true; // Enabled by default
    }

    public void Update(string name, string city, string address, string description)
    {
        Name = name;
        City = city;
        Address = address;
        Description = description;
    }

    public void ChangeStatus(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
