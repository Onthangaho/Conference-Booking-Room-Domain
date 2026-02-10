using Microsoft.EntityFrameworkCore;

public class EFBookingStore : IBookingStore
{
    private readonly ConferenceBookingDbContext _dbContext;

    public EFBookingStore(ConferenceBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    } //

    public async Task SaveAsync(Booking booking)
    {
        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Booking>> LoadBookingAsync()
    {
        return await _dbContext.Bookings.OrderByDescending(b => b.Start).ToListAsync();
    }
}