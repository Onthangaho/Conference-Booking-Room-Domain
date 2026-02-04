using System.ComponentModel.DataAnnotations;

public class CreateBookingDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
}