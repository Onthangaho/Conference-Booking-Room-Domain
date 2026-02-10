public interface IRoomStore
{
    Task <List<ConferenceRoom>> LoadRoomsAsync(CancellationToken cancellationToken=default);
    Task<ConferenceRoom?> LoadRoomByIdAsync(int id, CancellationToken cancellationToken=default);
}