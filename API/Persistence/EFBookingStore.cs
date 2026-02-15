using Microsoft.EntityFrameworkCore;

public class EFBookingStore : IBookingStore
{
  private readonly ConferenceBookingDbContext _dbContext;

    public EFBookingStore(ConferenceBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(Booking booking)
    {
        await _dbContext.Bookings.AddAsync(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _dbContext.Bookings.Update(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Booking booking)
    {
        _dbContext.Bookings.Remove(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _dbContext.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> HasOverlapAsync(int roomId, DateTime start, DateTime end)
    {
        return await _dbContext.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status == BookingStatus.Confirmed &&
            start < b.EndTime &&
            end > b.Start);
    }

    public Task<List<Booking>> LoadBookingAsync()
    {
        return _dbContext.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .Where(b => !b.IsDeleted)
            .ToListAsync();
    }
}