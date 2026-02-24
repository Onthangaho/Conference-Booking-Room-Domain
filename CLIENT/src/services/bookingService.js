// src/services/bookingService.js

// Fetch all bookings
export async function fetchAllBookings(signal) {
  const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bookings/all`, {
    signal,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`
    }
  });

  if (!response.ok) {
    throw new Error(`Failed to fetch bookings: ${response.status} ${response.statusText}`);
  }

  return await response.json();
}

// Add a new booking
export async function addBookingToService(newBooking) {
  const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bookings`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`
    },
    body: JSON.stringify(newBooking)
  });

  if (!response.ok) {
    throw new Error(`Failed to add booking: ${response.status} ${response.statusText}`);
  }

  return await response.json();
}

// Cancel a booking
export async function deleteBookingService(id) {
  const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bookings/${id}/cancel`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`
    },
    body: JSON.stringify({ id })
  });

  if (!response.ok) {
    throw new Error(`Failed to cancel booking: ${response.status} ${response.statusText}`);
  }

  return await response.json();
}