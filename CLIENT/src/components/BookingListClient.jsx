"use client";

import { memo } from "react";
import BookingCard from "./BookingCard";
// This component is responsible for displaying a list of bookings. It receives the bookings data, user role, and functions to handle booking deletion and editing as props. If there are no bookings to display, it shows a message indicating that no bookings were found. Otherwise, it maps over the bookings array and renders a BookingCard for each booking, passing down the necessary props to each card.
function BookingListClient({ bookings, role, deleteBooking, editBooking }) {
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

export default memo(BookingListClient);
