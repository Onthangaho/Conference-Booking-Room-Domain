import { useState, useEffect } from "react";
import { fetchAllBookings } from "../services/bookingService";
import { toast } from "react-toastify";

export function useBookings() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  //this effect runs once when the component mounts to fetch all bookings from the server. It uses an AbortController to allow cancellation of the fetch request if the component unmounts before the request completes. The fetched data is stored in the bookings state, and loading and error states are updated accordingly. Additionally, a success toast notification is shown after successfully loading the bookings, and an error toast is shown if the fetch fails.
  useEffect(() => {
    const controller = new AbortController();

    const load = async () => {
      try {
        // Simulate network delay for better UX
        const data = await fetchAllBookings(controller.signal);
        setTimeout(()=>{
            setBookings(data);
            setLoading(false);
            toast.success("Bookings loaded successfully!"); // Show success toast after loading
        }, 1000)
      } catch (err) {
        if (err.name !== "AbortError") {
          setError(err.message);
          toast.error("Failed to load bookings.");
        }
      } finally {
        setLoading(false);
      }
    };

    load();
    return () => controller.abort();
  }, []);

  return { bookings, loading, error, setBookings };
}
