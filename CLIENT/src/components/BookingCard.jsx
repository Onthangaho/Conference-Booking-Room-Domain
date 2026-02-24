import Button from "./Button";

function BookingCard({ booking, deleteBooking }) {
  return (
    <div className="booking-card">
      <h3>{booking.roomName}</h3>
      <p><strong>Type:</strong> {booking.roomType}</p>
      <p><strong>Capacity:</strong> {booking.capacity}</p>
      <p><strong>Start:</strong> {new Date(booking.start).toLocaleString()}</p>
      <p><strong>End:</strong> {new Date(booking.endTime).toLocaleString()}</p>
      <p><strong>Status:</strong> {booking.status}</p>
      <p><strong>Created At:</strong> {new Date(booking.createdAt).toLocaleString()}</p>

      <p><strong>User:</strong> {booking.createdBy}</p>
      <p><strong>Cancelled At:</strong> {booking.cancelledAt}</p>

      <div className="card-actions">
        <Button label="Edit" variant="primary" />
        <Button
          label="Cancel"
          variant="danger"
          onClick={() => {
            if (window.confirm("Cancel this booking?")) {
              deleteBooking(booking.id);
            }
          }}
        />
      </div>
    </div>
  );
}

export default BookingCard;