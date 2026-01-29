public class Booking
{
    public Guid Id { get; set; }
    public ConferenceRoom Room { get; set; }
    public string Name { get; set; }
    public DateTime BookingTime { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfAttendees { get; set; }
    public BookingStatus Status { get;  set; }

public Booking() { }
    public Booking(ConferenceRoom room, BookingRequest request)
    {
        /*
         * FAIL-FAST VALIDATION
         * Invalid bookings are rejected immediately.
         */
        if (room == null)
            throw new Exception("Booking must reference an existing room.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("BookedBy cannot be empty.");

        if (request.BookingTime >= request.EndTime)
            throw new Exception("End time must be after start time.");

        if (request.NumberOfAttendees <= 0)
            throw new Exception("There must be at least one attendee.");

        if (request.NumberOfAttendees > room.Capacity)
            throw new Exception("Number of attendees exceeds room capacity.");

        Id = Guid.NewGuid();
        Room = room;
        Name = request.Name;
        BookingTime = request.BookingTime;
        EndTime = request.EndTime;
        NumberOfAttendees = request.NumberOfAttendees;

        // Initial status of booking is Pending
        Status = BookingStatus.Pending;
    }
    public void Confirmed()
    {
        if (Status == BookingStatus.Pending)
        {
            Status = BookingStatus.Confirmed;
        }
        else
        {
            throw new Exception("Only pending bookings can be confirmed.");
        }
    }
    public void Cancel()
    {
        if (Status == BookingStatus.Pending || Status == BookingStatus.Confirmed)
        {
            Status = BookingStatus.Cancelled;
        }
        else
        {
            throw new Exception("Only pending or confirmed bookings can be cancelled.");
        }
    }


}