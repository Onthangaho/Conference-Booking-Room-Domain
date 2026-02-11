
using Microsoft.EntityFrameworkCore;

public class RoomStore : IRoomStore
{
    private readonly ConferenceBookingDbContext _dbContext;

    public RoomStore(ConferenceBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ConferenceRoom?> LoadRoomByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ConferenceRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<List<ConferenceRoom>> LoadRoomsAsync(CancellationToken cancellationToken = default)
    {
     return await _dbContext.ConferenceRooms
     .AsNoTracking()
     .ToListAsync(cancellationToken)
     .ConfigureAwait(false);
    }
}