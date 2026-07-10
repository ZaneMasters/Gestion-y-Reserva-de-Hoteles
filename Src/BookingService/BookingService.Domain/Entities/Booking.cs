namespace BookingService.Domain.Entities;

public enum BookingStatus
{
    Confirmed,
    Cancelled
}

public class Booking
{
    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid GuestId { get; private set; }
    
    public DateTime ArrivalDate { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public int NumberOfGuests { get; private set; }
    
    public string EmergencyContactName { get; private set; } = string.Empty;
    public string EmergencyContactPhone { get; private set; } = string.Empty;
    
    public BookingStatus Status { get; private set; }

    public Guest Guest { get; private set; } = null!;

    // For EF Core
    protected Booking() { }

    public Booking(Guid hotelId, Guid roomId, Guid guestId, DateTime arrivalDate, 
        DateTime departureDate, int numberOfGuests, string emergencyContactName, string emergencyContactPhone)
    {
        Id = Guid.NewGuid();
        HotelId = hotelId;
        RoomId = roomId;
        GuestId = guestId;
        ArrivalDate = arrivalDate;
        DepartureDate = departureDate;
        NumberOfGuests = numberOfGuests;
        EmergencyContactName = emergencyContactName;
        EmergencyContactPhone = emergencyContactPhone;
        Status = BookingStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = BookingStatus.Cancelled;
    }
}
