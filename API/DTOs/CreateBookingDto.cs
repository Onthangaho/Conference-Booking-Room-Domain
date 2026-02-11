using System.ComponentModel.DataAnnotations;

public class CreateBookingDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    /// <example>2026-02-11T09:00:00</example>

    public DateTime Start { get; set; }

    [Required]
    /// <example>2026-02-11T10:00:00</example>
    public DateTime EndTime { get; set; }
}