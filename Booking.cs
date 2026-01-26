public class Booking
{
    public string Name { get; set; }
    public DateTime BookingTime { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfAttendees { get; set; }
    public BookingStatus Status { get; private set; }

    public Booking(string name, DateTime bookingTime, DateTime endTime, int numberOfAttendees)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Name cannot be null or empty.");
        }

        if (endTime <= bookingTime)
        {
            throw new Exception("End time must be after booking time.");
        }

        if (numberOfAttendees <= 0)
        {
            throw new Exception("Number of attendees must be greater than zero.");
        }
        Name = name;
        BookingTime = bookingTime;
        EndTime = endTime;
        NumberOfAttendees = numberOfAttendees;
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