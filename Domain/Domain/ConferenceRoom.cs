public class ConferenceRoom
{
    public int Id { get;set; }
    public string Name { get; set; }
    public RoomType RoomType { get; set; }

  
    public int Capacity { get; set; }

    public ConferenceRoom()
    {
        
    }

   
    private readonly List<Booking> _bookings = new List<Booking>();

    public ConferenceRoom(int id, string name,int capacity, RoomType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name cannot be null or empty.");
        }

        if (capacity <= 0)
        {
            throw new Exception("Capacity must be a positive integer.");
        }
        
        Id =id;
        Name = name;
        RoomType = type;
        Capacity = capacity;

    }

  
    public IReadOnlyList<Booking> GetBookings()
    {
        return _bookings.AsReadOnly();
    }

    
 
    public bool HasBookings()
    {
        return _bookings.Any();
    }

    // Get the most recent booking
    public Booking? GetLastBooking()
    {
        return _bookings.LastOrDefault();
    }
    
    public void ClearBookings()
    {
        _bookings.Clear();
    }

    
    public void AttachBooking(Booking booking)
    {
        _bookings.Add(booking);
    }

}