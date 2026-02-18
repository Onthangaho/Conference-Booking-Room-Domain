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

    //track validation errors
    const [errors, setErrors] = useState({});
    const handleSubmit = (e) => {
        e.preventDefault();
        const newErrors = {};

        if (!roomName) newErrors.roomName = "Room Name is required";
        if (!roomType) newErrors.roomType = "Room Type is required";
        if (!location) newErrors.location = "Location is required";
        if (!userName) newErrors.userName = "User Name is required";
        if (!date) newErrors.date = "Date is required";
        if (!startTime) newErrors.startTime = "Start Time is required";
        if (!endTime) newErrors.endTime = "End Time is required";
        setErrors(newErrors);

        if (Object.keys(newErrors).length > 0) {
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
        handleClear();


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
        setErrors({});
    };

    return (

        <form onSubmit={handleSubmit} className="booking-form">

            <div>
                <select
                    className={errors.roomName ? "input-error" : ""}
                    value={roomName}
                    onChange={(e) => setRoomName(e.target.value)}
                >
                    <option value="">Select Room</option>
                    <option value="Conference Room A">Conference Room A</option>
                    <option value="Conference Room B">Conference Room B</option>
                    <option value="Conference Room C">Conference Room C</option>
                    <option value="Conference Room D">Conference Room D</option>
                </select>
                {errors.roomName && <p className="error-text">{errors.roomName}</p>}
            </div>

            <div>
                <select
                    className={errors.roomType ? "input-error" : ""}
                    value={roomType}
                    onChange={(e) => setRoomType(e.target.value)}
                >
                    <option value="">Select Room Type</option>
                    <option value="Training">Training</option>
                    <option value="Boardroom">Boardroom</option>
                    <option value="Standard">Standard</option>
                </select>
                {errors.roomType && <p className="error-text">{errors.roomType}</p>}
            </div>


            <div>
                <select
                    className={errors.location ? "input-error" : ""}
                    value={location}
                    onChange={(e) => setLocation(e.target.value)}
                >
                    <option value="">Select Location</option>
                    <option value="Bloemfontein">Bloemfontein</option>
                    <option value="Cape Town">Cape Town</option>
                </select>
                {errors.location && <p className="error-text">{errors.location}</p>}
            </div>

            <div>
                <input
                    className={errors.userName ? "input-error" : ""}
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    placeholder="User Name"
                />
                {errors.userName && <p className="error-text">{errors.userName}</p>}
            </div>


            <div>
                <label>Date</label>
                <input
                    type="date"
                    className={errors.date ? "input-error" : ""}
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                />
                {errors.date && <p className="error-text">{errors.date}</p>}
            </div>


            <div>
                <label>Start Time</label>
                <input
                    type="time"
                    className={errors.startTime ? "input-error" : ""}
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                />
                {errors.startTime && <p className="error-text">{errors.startTime}</p>}
            </div>

            <div>
                <label>End Time</label>
                <input
                    type="time"
                    className={errors.endTime ? "input-error" : ""}
                    value={endTime}
                    onChange={(e) => setEndTime(e.target.value)}
                />
                {errors.endTime && <p className="error-text">{errors.endTime}</p>}
            </div>


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