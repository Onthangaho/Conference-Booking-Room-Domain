public class BookingSummaryDto
{
    public int Id { get; set; }
    public string Room { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
}