namespace ConferenceBookingRoomDomain
{
    public record BookingRequest
    {
        public ConferenceRoom Room { get; }
        public DateTime Start { get; }

        public DateTime EndTime { get; }

  public BookingRequest( ConferenceRoom room, DateTime start, DateTime endTime)
        {
            Room = room;
            Start = start;
            EndTime = endTime;
        }

    }
}