import { useState } from "react";
import Button from "./Button";
import { useRooms } from "../hooks/useRooms";

function BookingForm({ addBooking, errors }) {
  const { rooms, loading } = useRooms();
  const safeRooms = Array.isArray(rooms) ? rooms : [];
  const [roomId, setRoomId] = useState("");
  const [date, setDate] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [localErrors, setLocalErrors] = useState({});

  const mergedErrors = { ...errors, ...localErrors };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const nextErrors = {};

    if (!roomId) nextErrors.roomId = "Please select a conference room.";
    if (!date) nextErrors.date = "Please select a date.";
    if (!startTime) nextErrors.start = "Please select a start time.";
    if (!endTime) nextErrors.endTime = "Please select an end time.";

    if (!nextErrors.start && !nextErrors.endTime && date) {
      const start = new Date(`${date}T${startTime}`);
      const end = new Date(`${date}T${endTime}`);
      const now = new Date();

      if (start >= end) {
        nextErrors.endTime = "End time must be after start time.";
      }

      if (start < now) {
        nextErrors.start = "Start time cannot be in the past.";
      }
    }

    setLocalErrors(nextErrors);

    if (Object.keys(nextErrors).length > 0) return;

    const start = new Date(`${date}T${startTime}`);
    const end = new Date(`${date}T${endTime}`);

    const newBooking = {
      roomId: parseInt(roomId, 10),
      start,
      endTime: end,
    };

    const success = await addBooking(newBooking);
    if (success) {
      handleClear();
    }
  };

  const handleClear = () => {
    setRoomId("");
    setDate("");
    setStartTime("");
    setEndTime("");
    setLocalErrors({});
  };

  return (
    <form onSubmit={handleSubmit} className="booking-form">
      <div>
        <label>Conference Room</label>
        <select
          className={mergedErrors.roomId ? "input-error" : ""}
          value={roomId}
          onChange={(e) => setRoomId(e.target.value)}
          disabled={loading}
        >
          <option value="">Select Room</option>
          {!loading &&
            safeRooms.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name} ({room.roomType}, {room.location})
              </option>
            ))}
        </select>
        {mergedErrors.roomId && <p className="error-text">{mergedErrors.roomId}</p>}
      </div>

      <div>
        <label>Date</label>
        <input
          type="date"
          className={mergedErrors.date ? "input-error" : ""}
          value={date}
          onChange={(e) => setDate(e.target.value)}
        />
        {mergedErrors.date && <p className="error-text">{mergedErrors.date}</p>}
      </div>

      <div>
        <label>Start Time</label>
        <input
          type="time"
          className={mergedErrors.start ? "input-error" : ""}
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          step="60"
        />
        {mergedErrors.start && <p className="error-text">{mergedErrors.start}</p>}
      </div>

      <div>
        <label>End Time</label>
        <input
          type="time"
          className={mergedErrors.endTime ? "input-error" : ""}
          value={endTime}
          onChange={(e) => setEndTime(e.target.value)}
          step="60"
        />
        {mergedErrors.endTime && <p className="error-text">{mergedErrors.endTime}</p>}
      </div>

      <div className="form-actions">
        <button type="submit" className="btn primary">Add Booking</button>
        <Button label="Clear" variant="danger" onClick={handleClear} type="button" />
      </div>
    </form>
  );
}

export default BookingForm;