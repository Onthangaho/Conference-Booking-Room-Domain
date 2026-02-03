public class ConferenceRoom
{
    public int Id { get;}
    public string Name { get; }
    public RoomType RoomType { get; }

    // Capacity based on room type
   /* public int Capacity
    {
        get
        {
            if (RoomType == RoomType.Small)
            {
                return 4;
            }
            else if (RoomType == RoomType.Medium)
            {
                return 10;
            }
            else
            {
                return 12;
            }
        }
    }*/
    public int Capacity { get; }

   
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

    // Determine room type based on number of attendees

    /*
   * DEFENSIVE COPY
   * Returns a snapshot, not the real list.
   */
    public IReadOnlyList<Booking> GetBookings()
    {
        return _bookings.AsReadOnly();
    }

    
    /*
  * CORE BUSINESS RULE:
  * A room cannot be double-booked.
  */
  // Try to add a booking to the room if available
  
    // Check if there are any bookings
    public bool HasBookings()
    {
        return _bookings.Any();
    }

    // Get the most recent booking
    public Booking? GetLastBooking()
    {
        return _bookings.LastOrDefault();
    }

   // Helper to clear bookings when reloading from file
    public void ClearBookings()
    {
        _bookings.Clear();
    }

    // Helper to attach a booking when reloading from file
    public void AttachBooking(Booking booking)
    {
        _bookings.Add(booking);
    }

}