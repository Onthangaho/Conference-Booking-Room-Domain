
public class BookingDeleteConflictException : Exception
{
    public BookingDeleteConflictException(int id)
        : base($"Booking {id} cannot be deleted because it is not cancelled.") { }
}
