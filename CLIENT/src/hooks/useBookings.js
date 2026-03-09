import { useState, useEffect } from "react";
import { fetchBookings, fetchMyBookings, createBooking, updateBooking, cancelBooking } from "../services/api";
import { parseValidationErrors } from "../utils/parseValidationErrors";
import { toast } from "react-toastify";
import axios from "axios";

export function useBookings(role) {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    const controller = new AbortController();

    const load = async () => {
      try {
        const data = role === "employee"
          ? await fetchMyBookings(controller.signal)
          : await fetchBookings(controller.signal);

        setBookings(Array.isArray(data) ? data : []);
      } catch (err) {
        if (axios.isCancel(err) || err?.code === "ERR_CANCELED") return;
        toast.error("Failed to load bookings");
      } finally {
        setLoading(false);
      }
    };

    load();
    return () => controller.abort();
  }, [role]);

  const addBooking = async (booking) => {
    try {
      const created = await createBooking(booking);
      setBookings((prev) => [...prev, created]);
      toast.success("Booking created!");
      setErrors({});
    } catch (err) {
      setErrors(parseValidationErrors(err.response?.data));
    }
  };

  const editBooking = async (id, booking) => {
    try {
      const updated = await updateBooking(id, booking);
      setBookings((prev) => prev.map((b) => (b.id === id ? updated : b)));
      toast.success("Booking updated!");
      setErrors({});
    } catch (err) {
      setErrors(parseValidationErrors(err.response?.data));
    }
  };

  const deleteBooking = async (id) => {
    try {
      const cancelled = await cancelBooking(id);
      setBookings((prev) => prev.map((b) => (b.id === id ? cancelled : b)));
      toast.success("Booking cancelled!");
    } catch (err) {
      toast.error("Failed to cancel booking");
    }
  };

  return { bookings, loading, errors, addBooking, editBooking, deleteBooking };
}