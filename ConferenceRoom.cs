public class ConferenceRoom
{
    public string Name { get; set; }
    public RoomType RoomType { get; set; }

    // Capacity based on room type
    public int capacity
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

    public ConferenceRoom(string name, RoomType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name cannot be null or empty.");
        }
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

    public void ValidateBooking(Booking booking)
    {
         
        if (booking.NumberOfAttendees > capacity)
        {
            throw new Exception(
                $"Room {Name} cannot accommodate {booking.NumberOfAttendees} attendees. Maximum capacity is {capacity}.");
        }
}
}