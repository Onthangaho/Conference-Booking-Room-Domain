public class InvalidBookingTimeException : Exception
{
    public InvalidBookingTimeException(DateTime start, DateTime end)
        : base($"Start time {start} must be before end time {end}.") { }
}
