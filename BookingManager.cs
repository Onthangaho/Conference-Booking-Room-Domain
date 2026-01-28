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

    /*
     * Adds rooms to the system.
     */
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
        return _rooms.ToList();
    }

    /*
     * Processes a booking request end-to-end.
     * This method clearly shows how decisions are made.
     */
    public bool TryProcessBooking(Guid roomId, BookingRequest request)
    {
        /*
         * LINQ AS A QUESTION:
         * Does the room exist?
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
        return _allBookings.ToList();
    }

    /*
     * GROUPING BOOKINGS BY STATUS
     * Demonstrates Dictionary usage.
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
