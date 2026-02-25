import { useState, useEffect } from "react";
import { fetchAllBookings, addBookingToService, deleteBookingService } from "../services/bookingService";
import { toast } from "react-toastify";
import axios from "axios";

export function useBookings() {
  // State variables to hold the list of bookings, loading status, and any error messages.
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // useEffect hook to fetch bookings when the component mounts. It also sets up an AbortController to allow for cancellation of the request if the component unmounts before the request completes.
  useEffect(() => {
    // Create an AbortController instance to manage cancellation of the fetch request.
    const controller = new AbortController();

    // Async function to load bookings from the API. It handles various error scenarios, such as request cancellation, timeouts, network errors, and server errors, providing appropriate feedback to the user through toast notifications.
    const load = async () => {
      try {
        const data = await fetchAllBookings(controller.signal);
        setBookings(data);
        toast.success("Bookings loaded successfully!");
      } catch (err) {
        //if the error is due to request cancellation, we log it and do not show an error toast, as this is an expected scenario when the component unmounts. For other types of errors, we set the error state and show a toast notification with the relevant message.
        if (axios.isCancel(err)) {
          console.log("Request cancelled");

        } else if (err.code === "ECONNABORTED") {
          setError("Server took too long to respond (timeout).");
          toast.error("Timeout: server took too long.");
        } else if (err.message.includes("Network Error")) {
          setError("Server unreachable. Please check your connection.");
          toast.error("Network error: server unreachable.");
        } else if (err.response) {
          setError(`Server error ${err.response.status}: ${err.response.data}`);
          toast.error(`Server error ${err.response.status}`);
        } else {
          setError("Unexpected error occurred.");
          toast.error("Unexpected error occurred.");
        }
      } finally {
        setLoading(false);
      }
    };

    load();
    return () => controller.abort();
  }, []);

  // Function to add a new booking. It sends the booking data to the API and updates the local state with the newly created booking. It also provides feedback to the user through toast notifications based on the success or failure of the operation.
  const addBooking = async (newBooking) => {
    try {
      const created = await addBookingToService(newBooking);
      setBookings((prev) => [...prev, created]);
      toast.success("Booking added successfully!");
    } catch (err) {
      toast.error(err.message);
    }
  };
// Function to delete (cancel) a booking. It sends a request to the API to cancel the booking by its ID and updates the local state to reflect the cancellation. It also provides feedback to the user through toast notifications based on the success or failure of the operation.
  const deleteBooking = async (id) => {
    try {
      const cancelled = await deleteBookingService(id);
      setBookings((prev) =>
        prev.map((b) => (b.id === id ? { ...b, ...cancelled } : b))
      );
      toast.success("Booking cancelled successfully!");
    } catch (err) {
      toast.error(err.message);
    }
  };

  return { bookings, loading, error, addBooking, deleteBooking };
}