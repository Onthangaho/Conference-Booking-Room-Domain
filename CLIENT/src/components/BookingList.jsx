import BookingCard from "./BookingCard";

function BookingList({ bookings, role, deleteBooking, editBooking }) {
  if (!bookings || bookings.length === 0) {
    return <p className="no-bookings">No bookings found.</p>;
  }

  return (
    <div className="bookings-list">
      {bookings.map((booking) => (
        <BookingCard
          key={booking.id}
          booking={booking}
          role={role}
          deleteBooking={deleteBooking}
          editBooking={editBooking}
        />
      ))}
    </div>
  );
}

export default BookingList;