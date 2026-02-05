using System.ComponentModel.DataAnnotations;

public class CancelBookingDto
    {
        [Required(ErrorMessage = "Booking Id is required")]
        public int Id { get; set; }
    }
