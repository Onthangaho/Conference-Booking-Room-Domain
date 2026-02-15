using ConferenceBookingRoomDomain;
public interface IBookingStore
{
    Task SaveAsync(Booking booking);
    Task<List<Booking>> LoadBookingAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(Booking booking);
    Task<bool> HasOverlapAsync(int roomId, DateTime start, DateTime end);
}