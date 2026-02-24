import Heading from "./Heading";
import BookingList from "./BookingList";
import BookingForm from "./BookingForm";
import Button from "./Button";
import { useState, useEffect } from "react";

function Dashboard({ bookings, addBooking, deleteBooking }) {
  const [category, setCategory] = useState("All");
  const [filteredBookings, setFilteredBookings] = useState(bookings);

  useEffect(() => {
    if (category === "All") {
      setFilteredBookings(bookings);
    } else {
      setFilteredBookings(bookings.filter((b) => b.roomType === category));
    }
  }, [category, bookings]);

  return (
    <main className="container">
      <p className="counter"><strong>Total Bookings: {filteredBookings.length}</strong></p>

      <Heading title="Add a New Booking" />
      <BookingForm addBooking={addBooking} />

      <div className="filter-section">
        <label htmlFor="category">Filter by Room Type:</label>
        <select id="category" value={category} onChange={(e) => setCategory(e.target.value)}>
          <option value="All">All</option>
          <option value="Training">Training</option>
          <option value="BoardRoom">Boardroom</option>
          <option value="Standard">Standard</option>
        </select>
      </div>

      {filteredBookings.length > 0 ? (
        <BookingList bookings={filteredBookings} deleteBooking={deleteBooking} />
      ) : (
        <p className="no-bookings">No bookings found for this filter.</p>
      )}
    </main>
  );
}

export default Dashboard;