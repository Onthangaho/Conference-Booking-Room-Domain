public class ConferenceRoom
{
    public int Id { get; }
    public string Name { get; }
    public RoomType RoomType { get; }

    // Capacity based on room type
    public int Capacity
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
    }

   
    private readonly List<Booking> _bookings = new List<Booking>();

    public ConferenceRoom(string name, RoomType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name cannot be null or empty.");
        }
        Id = Guid.NewGuid();
        Name = name;
        RoomType = type;

    }

    // Determine room type based on number of attendees
    public static RoomType GetRoomType(int numberOfAttendees)
    {
        if (numberOfAttendees <= 4)
        {
            return RoomType.Small;
        }
        else if (numberOfAttendees <= 10)
        {
            return RoomType.Medium;
        }
        else
        {
            return RoomType.Large;
        }
    }
    /*
   * DEFENSIVE COPY
   * Returns a snapshot, not the real list.
   */
    public IReadOnlyList<Booking> GetBookings()
    {
        return _bookings.AsReadOnly();
    }

      public bool IsAvailable(DateTime start, DateTime end)
    {
        return !_bookings.Any(existing =>
            existing.Status == BookingStatus.Confirmed &&
            start < existing.EndTime &&
            end > existing.BookingTime
        );
    }
    /*
  * CORE BUSINESS RULE:
  * A room cannot be double-booked.
  */
  // Try to add a booking to the room if available
    public bool TryAddBooking(Booking booking)
    {
          if (!IsAvailable(booking.BookingTime, booking.EndTime))
        {
            booking.Cancel();
            return false;
        }

        booking.Confirmed();
        _bookings.Add(booking);
        return true;
    }
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


}