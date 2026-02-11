public class SessionResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public int Capacity { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}