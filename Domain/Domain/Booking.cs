public class Booking
{
    // public Guid Id { get; set; }
    public ConferenceRoom Room { get; }
    // public string Name { get;  }
    public DateTime Start { get; }
    public DateTime EndTime { get; }
    // public int NumberOfAttendees { get;  }
    public BookingStatus Status { get; private set; }

    //ublic Booking() { }

    public Booking(ConferenceRoom room, DateTime start, DateTime endTime)
    {
   
        // Id = Guid.NewGuid();
        Room = room;
        Start = start;
        EndTime = endTime;

        
        //Status = BookingStatus.Pending;
    }
    public void Confirm()
    {


        Status = BookingStatus.Confirmed;


    }
    public void Cancel()
    {


        Status = BookingStatus.Cancelled;


    }


}