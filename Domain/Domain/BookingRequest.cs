namespace ConferenceBookingRoomDomain
{
    public record BookingRequest
    {
        public int RoomId { get; }
        public string UserId { get; }
        public DateTime Start { get; }

        public DateTime EndTime { get; }

  public BookingRequest(int roomId, string userId, DateTime start, DateTime endTime)
        {
            RoomId = roomId;
            UserId = userId;
            Start = start;
            EndTime = endTime;
        }

    }
}