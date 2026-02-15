using ConferenceBookingRoomDomain.Domain;
public class Booking
{
    
    // public Guid Id { get; set; }
    public int Id { get;  set; }

    public int RoomId { get; set; }
    public ConferenceRoom Room { get; set; }= null!;
    // public string Name { get;  }
    public DateTime Start { get; set; }
    public DateTime EndTime { get; set; }
    // public int NumberOfAttendees { get;  }
    public BookingStatus Status { get; set; }= BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAt { get; set; }

    public string? UserId { get; set; } 
    public ApplicationUser? User { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    

    public Booking()
    {
       

    }


    public Booking(int roomId,string userId, DateTime start, DateTime endTime)
    {

        // Id = Guid.NewGuid();
       
         RoomId = roomId;
        UserId = userId;
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
   public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }



}