import Button from "./Button";
import { useState } from "react";
import ConfirmModal from "./ComfirmModal";

function BookingCard({ booking, deleteBooking }) {

  const [showConfirm, setShowConfirm] = useState(false);
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
      

      <div className="card-actions">
        <Button label="Edit" variant="primary" />
        <Button
          label="Cancel"
          variant="danger"
          onClick={() => setShowConfirm(true)}
        />
      </div>

      {showConfirm && (
        <ConfirmModal
          message="Are you sure you want to cancel this booking?"
          onConfirm={() => {
            deleteBooking(booking.id);
            setShowConfirm(false);
          }}
          onCancel={() => setShowConfirm(false)}
        />
      )}



    </div>
  );
}

export default BookingCard;