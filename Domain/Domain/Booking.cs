public class Booking
{
    
    // public Guid Id { get; set; }
    public int Id { get;  set; }
    public ConferenceRoom Room { get; set; }
    // public string Name { get;  }
    public DateTime Start { get; set; }
    public DateTime EndTime { get; set; }
    // public int NumberOfAttendees { get;  }
    public BookingStatus Status { get; set; }= BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }

    public string UserId { get; set; }
    public ApplicationUSer User { get; set; }

    

    public Booking()
    {
       

    }


    public Booking(ConferenceRoom room, DateTime start, DateTime endTime)
    {

        // Id = Guid.NewGuid();
       
        Room = room;
        Start = start;
        EndTime = endTime;
        Status = BookingStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        


        //Status = BookingStatus.Pending;
    }
    public void Confirm()
    {


        Status = BookingStatus.Confirmed;


    }
    public void Cancel()
    {


        Status = BookingStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;


    }


}