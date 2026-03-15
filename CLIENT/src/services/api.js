import apiClient from "../api/apiClient";

// Fetch all bookings
export async function fetchBookings(signal, searchTerm = "") {
  return await apiClient.get("/Bookings/all", {
    signal,
    params: searchTerm ? { searchTerm } : undefined,
  });
}

export async function fetchMyBookings(signal, searchTerm = "") {
  return await apiClient.get("/Bookings/mine", {
    signal,
    params: searchTerm ? { searchTerm } : undefined,
  });
}

export async function createBooking(booking) {
  // Payload must match CreateBookingDto: { roomId, start, endTime }
  return await apiClient.post("/Bookings", booking);
}

export async function updateBooking(id, booking) {
  return await apiClient.put(`/Bookings/${id}`, booking);
}

export async function cancelBooking(id) {
  return await apiClient.put(`/Bookings/${id}/cancel`, { id });
}