namespace ConferenceBookingRoomDomain
{
    public record BookingRequest
    {
        public ConferenceRoom Room { get; }
        public string UserId { get; }
        public DateTime Start { get; }

        public DateTime EndTime { get; }

  public BookingRequest( ConferenceRoom room, string userId, DateTime start, DateTime endTime)
        {
            Room = room;
            UserId = userId;
            Start = start;
            EndTime = endTime;
        }

    }
}