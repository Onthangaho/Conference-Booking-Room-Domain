/*
 * Coordinates bookings across the system.
 * Applies business rules that span multiple collections.
 */
public class BookingManager
{
    /*
     * INTERNAL MUTABLE STATE
     */
    private readonly List<ConferenceRoom> _rooms = new();
    private readonly List<Booking> _allBookings = new();

    public BookingManager()
    {
    }

    public void SeedData()
    {
        ConferenceRoom room1 = new ConferenceRoom(1001, "Small Room", RoomType.Small);
        ConferenceRoom room2 = new ConferenceRoom(1002, "Medium Room", RoomType.Medium);
        ConferenceRoom room3 = new ConferenceRoom(1003, "Large Room", RoomType.Large);

        Booking seededBooking = new Booking(room2,
            new BookingRequest("Alice", DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 4));
        room1.TryAddBooking(seededBooking);
        _allBookings.Add(seededBooking);
    }
   
    public void AddRoom(ConferenceRoom room)
    {
        if (room == null)
            throw new Exception("Room cannot be null.");

        _rooms.Add(room);
    }

    /*
     * DEFENSIVE COPY
     */
    public IReadOnlyList<ConferenceRoom> GetRooms()
    {
        return _rooms.AsReadOnly();
    }

    /*
     * Processes a booking request end-to-end.
     
     */
    public bool TryProcessBooking(int roomId, BookingRequest request)
    {
        /*
         * LINQ AS A QUESTION:
            * "Find me the room with this ID."
         */
        ConferenceRoom? room =
            _rooms.FirstOrDefault(r => r.Id == roomId);

        if (room == null)
            throw new Exception("Selected room does not exist.");

        // Booking validates itself
        Booking booking = new Booking(room, request);

        bool approved = room.TryAddBooking(booking);

        _allBookings.Add(booking);
        return approved;
    }

    /*
     * DEFENSIVE COPY
     */
    public IReadOnlyList<Booking> GetAllBookings()
    {
        return _allBookings.AsReadOnly();
    }

    /*
     * GROUPING BOOKINGS BY STATUS
     
     */
    public Dictionary<BookingStatus, List<Booking>> GroupBookingsByStatus()
    {
        var grouped = new Dictionary<BookingStatus, List<Booking>>();

        foreach (var booking in _allBookings)
        {
            if (!grouped.ContainsKey(booking.Status))
            {
                grouped[booking.Status] = new List<Booking>();
            }

            grouped[booking.Status].Add(booking);
        }

        return grouped;
    }
}
