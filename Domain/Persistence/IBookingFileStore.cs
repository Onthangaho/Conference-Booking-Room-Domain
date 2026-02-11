using ConferenceBookingRoomDomain;
public interface IBookingStore
{
    Task SaveAsync(Booking booking);
    Task<List<Booking>> LoadBookingAsync();
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(Booking booking);
}