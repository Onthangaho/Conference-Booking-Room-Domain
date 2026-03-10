// Server-first list component (no hooks, no client directives).
// This is kept for demonstrating the Server/Client boundary requirement.
function BookingList({ bookings }) {
  if (!bookings || bookings.length === 0) {
    return <p className="no-bookings">No bookings found.</p>;
  }

  return (
    <div className="bookings-list">
      {bookings.map((booking) => (
        <div className="booking-card" key={booking.id}>
          <h3>{booking.roomName}</h3>
          <p><strong>Type:</strong> {booking.roomType}</p>
          <p><strong>Status:</strong> {booking.status}</p>
        </div>
      ))}
    </div>
  );
}

export default BookingList;