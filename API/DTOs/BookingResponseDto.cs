public class BookingResponseDto
{
    public int Id { get; set; }
    public string? RoomName { get; set; }
    public string? RoomType { get; set; }
    public int Capacity { get; set; }
    public DateTime Start { get; set; }
    public DateTime EndTime { get; set; }
    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CancelledAt { get; set; }
    public string? CreatedBy { get; set; }
    public bool IsCancelled { get; set; } 
}