public class Booking
{
    private static int _idCounter = 1;//created a simple count to assigned to each booking as Id
    // public Guid Id { get; set; }
    public int Id { get;  set; }
    public ConferenceRoom Room { get; set; }
    // public string Name { get;  }
    public DateTime Start { get; set; }
    public DateTime EndTime { get; set; }
    // public int NumberOfAttendees { get;  }
    public BookingStatus Status { get; set; }

    public Booking()
    {
       

    }


    public Booking(ConferenceRoom room, DateTime start, DateTime endTime)
    {

        // Id = Guid.NewGuid();
        Id = _idCounter++;
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