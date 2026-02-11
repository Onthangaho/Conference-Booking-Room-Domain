public class ConferenceRoomResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? RoomType { get; set; }
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }

    public string Availability => IsActive ? "Available" : "Unavailable";
}
