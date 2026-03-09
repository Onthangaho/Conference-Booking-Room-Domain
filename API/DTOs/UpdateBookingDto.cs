using System.ComponentModel.DataAnnotations;

public class UpdateBookingDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
}
