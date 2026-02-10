public interface IRoomStore
{
    Task <List<ConferenceRoom>> LoadRoomsAsync();
    Task<ConferenceRoom> GetRoomAsync(int roomId);
}//