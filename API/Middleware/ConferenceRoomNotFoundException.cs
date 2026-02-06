public class ConferenceRoomNotFoundException : Exception
{
    public ConferenceRoomNotFoundException(int id)
        : base($"Conference room with ID {id} not found.") { }
}
