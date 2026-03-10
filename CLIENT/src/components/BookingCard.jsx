"use client";

import Button from "./Button";
import { useState } from "react";
import ConfirmModal from "./ComfirmModal";
import BookingDetailsModal from "./BookingDetailsModal";

function BookingCard({ booking, role, deleteBooking, editBooking }) {

  const [showDetails, setShowDetails] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editDate, setEditDate] = useState("");
  const [editStartTime, setEditStartTime] = useState("");
  const [editEndTime, setEditEndTime] = useState("");
  const [editError, setEditError] = useState("");

  const isAdmin = role === "admin";
  const isEmployee = role === "employee" || role === "receptionist";
  const canEdit = isEmployee && !booking.isCancelled;

  const formatDateTime = (value) => {
    const date = new Date(value);
    return date.toLocaleString([], {
      year: "numeric",
      month: "short",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  };

  const handleStartEdit = () => {
    const start = new Date(booking.start);
    const end = new Date(booking.endTime);

    setEditDate(start.toISOString().slice(0, 10));
    setEditStartTime(start.toTimeString().slice(0, 5));
    setEditEndTime(end.toTimeString().slice(0, 5));
    setEditError("");
    setIsEditing(true);
  };

  const handleSaveEdit = async () => {
    if (!editDate || !editStartTime || !editEndTime) {
      setEditError("Please fill date, start time, and end time.");
      return;
    }

    const start = new Date(`${editDate}T${editStartTime}`);
    const end = new Date(`${editDate}T${editEndTime}`);

    if (start >= end) {
      setEditError("End time must be after start time.");
      return;
    }

    if (start < new Date()) {
      setEditError("Start time cannot be in the past.");
      return;
    }

    const success = await editBooking(booking.id, {
      roomId: booking.roomId,
      start,
      endTime: end,
    });

    if (success) {
      setIsEditing(false);
      setEditError("");
    }
  };

  return (
    <div className="booking-card">
      <h3>{booking.roomName}</h3>
      <p><strong>Type:</strong> {booking.roomType}</p>
      <p><strong>Capacity:</strong> {booking.capacity}</p>
      <p><strong>Start:</strong> {formatDateTime(booking.start)}</p>
      <p><strong>End:</strong> {formatDateTime(booking.endTime)}</p>
      <p><strong>Status:</strong> {booking.status}</p>
      <p><strong>Created At:</strong> {formatDateTime(booking.createdAt)}</p>

      <p><strong>User:</strong> {booking.createdBy}</p>

      {isEditing && (
        <div className="edit-panel">
          <h4>Edit Booking</h4>
          <div className="edit-grid">
            <input type="date" value={editDate} onChange={(e) => setEditDate(e.target.value)} />
            <input type="time" step="60" value={editStartTime} onChange={(e) => setEditStartTime(e.target.value)} />
            <input type="time" step="60" value={editEndTime} onChange={(e) => setEditEndTime(e.target.value)} />
          </div>
          {editError && <p className="error-text">{editError}</p>}
          <div className="card-actions">
            <Button label="Save" variant="primary" onClick={handleSaveEdit} />
            <Button label="Cancel Edit" variant="secondary" onClick={() => setIsEditing(false)} />
          </div>
        </div>
      )}

      <div className="card-actions">
        <Button label="View" variant="secondary" onClick={() => setShowDetails(true)} />
        {canEdit && <Button label="Edit" variant="primary" onClick={handleStartEdit} />}
        {isAdmin && (
          <Button
            label="Cancel"
            variant="danger"
            onClick={() => setShowConfirm(true)}
          />
        )}
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

      {showDetails && (
        <BookingDetailsModal
          booking={booking}
          onClose={() => setShowDetails(false)}
          formatDateTime={formatDateTime}
        />
      )}

    </div>
  );
}

export default BookingCard;