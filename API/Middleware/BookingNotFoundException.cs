public class BookingNotFoundException : Exception
{
    public BookingNotFoundException(int id)
        : base($"Booking with ID {id} was not found.") { }
}
