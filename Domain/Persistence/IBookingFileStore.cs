using ConferenceBookingRoomDomain;
public interface IBookingStore
{
    Task SaveAsync(IEnumerable<Booking> bookings);
    Task<List<Booking>> LoadBookingAsync();
}