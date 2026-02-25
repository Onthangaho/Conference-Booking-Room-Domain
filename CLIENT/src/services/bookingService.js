import apiClient from "../api/apiClient";

// This function will send a GET request to the API to fetch all bookings. The signal parameter is used to allow for cancellation of the request if needed, which can be useful in scenarios where the user navigates away from the page before the request completes.
export async function fetchAllBookings(signal) {

  return apiClient.get('/Bookings/all', { signal });
}
// This function will send a POST request to the API to create a new booking. The newBooking parameter is expected to be an object containing the necessary data for the booking, such as date, time, and any other relevant information. The API endpoint will handle the creation of the booking and return the created booking data in the response.
export async function addBookingToService(newBooking) {

  return apiClient.post('/Bookings', newBooking);
}

// This function will send a PUT request to the API to cancel a booking by its ID. The API endpoint is expected to handle the cancellation logic and update the booking status accordingly.
export async function deleteBookingService(id) {
 return apiClient.put(`/Bookings/${id}/cancel`, {id});
}