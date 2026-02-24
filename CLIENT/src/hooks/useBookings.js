import { useState, useEffect } from "react";
import { fetchAllBookings } from "../services/bookingService";
import { toast } from "react-toastify";

export function useBookings() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const controller = new AbortController();

    const load = async () => {
      try {
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
