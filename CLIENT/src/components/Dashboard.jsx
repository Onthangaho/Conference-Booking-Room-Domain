import Heading from "./Heading";
import BookingList from "./BookingList";
import BookingForm from "./BookingForm";
import { useState, useEffect } from "react";
import { useBookings } from "../hooks/useBookings";

function Dashboard() {
  const { bookings, loading, error, addBooking, deleteBooking } = useBookings();
  const [category, setCategory] = useState("All");
  const [filteredBookings, setFilteredBookings] = useState([]);

  useEffect(() => {
    if (category === "All") {
      setFilteredBookings(bookings);
    } else {
      setFilteredBookings(bookings.filter((b) => b.roomType === category));
    }
  }, [category, bookings]);

  if (loading) return <p>Loading bookings...</p>;
  //this error handling will display an error message and a retry button if there was an error fetching the bookings. The retry button will reload the page, which will trigger the useEffect to fetch the bookings again.
  if (error)
    return (
      <div className="error-state">
        <p>{error}</p>
        <button
          className="retry-button"
          onClick={() => window.location.reload()}
        >
          🔄 Retry
        </button>
      </div>
    );


  return (
    <main className="container">
      <p className="counter">
        <strong>Total Bookings: {filteredBookings.length}</strong>
      </p>

      <Heading title="Add a New Booking" />
      <BookingForm addBooking={addBooking} />

      <div className="filter-section">
        <label htmlFor="category">Filter by Room Type:</label>
        <select
          id="category"
          value={category}
          onChange={(e) => setCategory(e.target.value)}
        >
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