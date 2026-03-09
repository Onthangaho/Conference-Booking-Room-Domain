import ReactDOM from "react-dom";

function BookingDetailsModal({ booking, onClose, formatDateTime }) {
  return ReactDOM.createPortal(
    <div className="modal-overlay">
      <div className="modal-box details-modal">
        <h3>Booking Details</h3>
        <p><strong>Booking ID:</strong> {booking.id}</p>
        <p><strong>Room:</strong> {booking.roomName}</p>
        <p><strong>Room Type:</strong> {booking.roomType}</p>
        <p><strong>Capacity:</strong> {booking.capacity}</p>
        <p><strong>Start:</strong> {formatDateTime(booking.start)}</p>
        <p><strong>End:</strong> {formatDateTime(booking.endTime)}</p>
        <p><strong>Status:</strong> {booking.status}</p>
        <p><strong>Created At:</strong> {formatDateTime(booking.createdAt)}</p>
        <p><strong>Created By:</strong> {booking.createdBy}</p>

        <div className="modal-actions">
          <button className="btn secondary" onClick={onClose}>Close</button>
        </div>
      </div>
    </div>,
    document.body
  );
}

export default BookingDetailsModal;
