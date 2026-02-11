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

        _dbContext.ConferenceRooms.Attach(booking.Room); // Attach the room to the context to avoid duplicate entries
        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _dbContext.ConferenceRooms.Attach(booking.Room); // Attach the room to the context to avoid issues with foreign key constraints
        _dbContext.Bookings.Update(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Booking>> LoadBookingAsync()
    {
        return await _dbContext.Bookings
        .Include(b => b.Room) // Include the related ConferenceRoom entity
        .OrderByDescending(b => b.Start)
        .ToListAsync();
    }

    public async Task DeleteAsync(Booking booking)
    {
        _dbContext.ConferenceRooms.Attach(booking.Room); // Attach the room to the context to avoid issues with foreign key constraints
        _dbContext.Bookings.Remove(booking);
        await _dbContext.SaveChangesAsync();
    }
}