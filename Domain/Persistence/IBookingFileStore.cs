using ConferenceBookingRoomDomain;
public interface IBookingStore
{
    Task SaveAsync(Booking booking);
    Task<List<Booking>> LoadBookingAsync();
}