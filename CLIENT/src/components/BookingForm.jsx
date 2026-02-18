import { useState } from "react";
import Button from "./Button";


function BookingForm({ addBooking }) {

    const [roomName, setRoomName] = useState("");
    const [roomType, setRoomType] = useState("");
    const [location, setLocation] = useState("");
    const [userName, setUserName] = useState("");
    const [date, setDate] = useState("");
    const [startTime, setStartTime] = useState("");
    const [endTime, setEndTime] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!roomName || !roomType || !location || !userName || !date || !startTime || !endTime) {
            alert("Please fill in all fields");
            return;
        }

        const newBooking = {
            id: Date.now(),
            roomName,
            roomType,
            location,
            userName,
            date,
            startTime,
            endTime
        };

        addBooking(newBooking);
        alert("Booking added successfully!");


        // Clear form fields after submission
        setRoomName("");
        setRoomType("");
        setLocation("");
        setUserName("");
        setDate("");
        setStartTime("");
        setEndTime("");
    };

    const handleClear = () => {
        setRoomName("");
        setRoomType("");
        setLocation("");
        setUserName("");
        setDate("");
        setStartTime("");
        setEndTime("");
    };

    return (

        <form onSubmit={handleSubmit} className="booking-form">

            <input value={roomName} onChange={(e) => setRoomName(e.target.value)} placeholder="Room Name" />
            <select value={roomType} onChange={(e) => setRoomType(e.target.value)}>
                <option value="">Select Room Type</option>
                <option value="Training">Training</option>
                <option value="Boardroom">Boardroom</option>
                <option value="Standard">Standard</option>
            </select>

            <select value={location} onChange={(e) => setLocation(e.target.value)}>
                <option value="">Select Location</option>
                <option value="Bloemfontein">Bloemfontein</option>
                <option value="Cape Town">Cape Town</option>
            </select>
            <input value={userName} onChange={(e) => setUserName(e.target.value)} placeholder="User Name" />
            <input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
            <input type="time" value={startTime} onChange={(e) => setStartTime(e.target.value)} />
            <input type="time" value={endTime} onChange={(e) => setEndTime(e.target.value)} />

            <div className="form-actions">
                <button type="submit" className="btn primary">Add Booking</button>

                <Button
                    label="Clear"
                    variant="danger"
                    onClick={handleClear}
                    type="button"
                />

            </div>

        </form>
    );
}

export default BookingForm;