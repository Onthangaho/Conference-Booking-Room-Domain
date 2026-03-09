import { useState, useEffect } from "react";
import { fetchBookings, fetchMyBookings, createBooking, updateBooking, cancelBooking } from "../services/api";
import { parseValidationErrors } from "../utils/parseValidationErrors";
import { toast } from "react-toastify";
import axios from "axios";
import * as signalR from "@microsoft/signalr";

export function useBookings(role) {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errors, setErrors] = useState({});

  const isEmployeeView = role === "employee" || role === "receptionist";

  useEffect(() => {
    const controller = new AbortController();

    const load = async () => {
      try {
        const data = isEmployeeView
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
  }, [isEmployeeView]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const currentUsername = (localStorage.getItem("username") || "").toLowerCase();

    if (!token || !role) return;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_API_BASE_URL}/hubs/bookings`, {
        accessTokenFactory: () => localStorage.getItem("token") || "",
      })
      .withAutomaticReconnect()
      .build();

    connection.on("bookingChanged", (action, booking) => {
      setBookings((prev) => {
        if (!booking) return prev;

        const bookingId = booking.id || booking.Id;
        if (!bookingId) return prev;

        const normalizedBooking = {
          ...booking,
          id: bookingId,
        };

        const existingIndex = prev.findIndex((item) => item.id === bookingId);
        const bookingOwner = (normalizedBooking.createdBy || "").toLowerCase();
        const isMine = !isEmployeeView || bookingOwner === currentUsername;

        if (action === "deleted") {
          if (existingIndex === -1) return prev;
          return prev.filter((item) => item.id !== bookingId);
        }

        if (!isMine && existingIndex === -1) {
          return prev;
        }

        if (existingIndex >= 0) {
          const existing = prev[existingIndex];
          const updated = { ...existing, ...normalizedBooking };

          const isDuplicate =
            existing.start === updated.start &&
            existing.endTime === updated.endTime &&
            existing.status === updated.status &&
            existing.cancelledAt === updated.cancelledAt &&
            existing.isCancelled === updated.isCancelled;

          if (isDuplicate) return prev;

          return prev.map((item) => (item.id === bookingId ? updated : item));
        }

        if (action === "created") {
          return [...prev, normalizedBooking];
        }

        return prev;
      });
    });

    connection.start().catch(() => {
      toast.error("Realtime sync is unavailable. You can still use manual refresh.");
    });

    return () => {
      connection.off("bookingChanged");
      connection.stop();
    };
  }, [isEmployeeView, role]);

  const addBooking = async (booking) => {
    try {
      const created = await createBooking(booking);
      setBookings((prev) => [...prev, created]);
      toast.success("Booking created!");
      setErrors({});
      return true;
    } catch (err) {
      setErrors(parseValidationErrors(err.response?.data));
      toast.error(err.response?.data?.message || "Failed to create booking");
      return false;
    }
  };

  const editBooking = async (id, booking) => {
    try {
      const updated = await updateBooking(id, booking);
      setBookings((prev) => prev.map((b) => (b.id === id ? updated : b)));
      toast.success("Booking updated!");
      setErrors({});
      return true;
    } catch (err) {
      setErrors(parseValidationErrors(err.response?.data));
      toast.error(err.response?.data?.message || "Failed to update booking");
      return false;
    }
  };

  const deleteBooking = async (id) => {
    try {
      const cancelled = await cancelBooking(id);
      setBookings((prev) => prev.map((b) => (b.id === id ? cancelled : b)));
      toast.success("Booking cancelled!");
      return true;
    } catch (err) {
      toast.error("Failed to cancel booking");
      return false;
    }
  };

  return { bookings, loading, errors, addBooking, editBooking, deleteBooking };
}