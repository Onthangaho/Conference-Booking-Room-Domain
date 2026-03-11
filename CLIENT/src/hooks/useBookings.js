"use client";
// This hook manages booking data and real-time updates based on user role (employee or admin).
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

  // Load bookings on mount and when role changes
  useEffect(() => {
    const controller = new AbortController();
    // Fetch bookings based on user role
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
// Set up SignalR connection for real-time updates
  useEffect(() => {
    const token = localStorage.getItem("token");
    const currentUsername = (localStorage.getItem("username") || "").toLowerCase();
    // If no token or role, do not establish connection
    if (!token || !role) return;
    // Build SignalR connection with access token for authentication
    // Remove /api from base URL for SignalR endpoint since hub is at root level
    const baseUrl = (import.meta.env.VITE_API_BASE_URL ?? "").replace(/\/api$/, "") || "http://localhost:5248";
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}/hubs/bookings`, {
        accessTokenFactory: () => localStorage.getItem("token") || "",
      })
      .configureLogging(signalR.LogLevel.None)
      .withAutomaticReconnect()
      .build();
    let isDisposed = false;
    // Listen for booking changes and update state accordingly
    connection.on("bookingChanged", (action, booking) => {
      setBookings((prev) => {
        // If no booking data, return previous state
        if (!booking) return prev;
        //this is to handle the case where the API returns a booking object without an 'id' property, but has 'Id' instead. We want to normalize it to always have 'id'.
        const bookingId = booking.id || booking.Id;
        if (!bookingId) return prev;

        const normalizedBooking = {
          ...booking,
          id: bookingId,
        };
      // Find existing booking index in state
        const existingIndex = prev.findIndex((item) => item.id === bookingId);
        const bookingOwner = (normalizedBooking.createdBy || "").toLowerCase();
        const isMine = !isEmployeeView || bookingOwner === currentUsername;
      // Handle deletion: if booking is deleted, remove it from state
        if (action === "deleted") {
          if (existingIndex === -1) return prev;
          return prev.filter((item) => item.id !== bookingId);
        }
      // For non-deletion actions, if the booking is not relevant to the user (not theirs and they are in employee view), do not add it to state
        if (!isMine && existingIndex === -1) {
          return prev;
        }
 // If booking exists, update it; if not and it's a creation action, add it to state
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

    const startPromise = connection.start().catch((err) => {
      const message = err?.message || "";
      if (isDisposed || message.includes("stopped during negotiation")) return;
      toast.error("Realtime sync is unavailable. You can still use manual refresh.");
    });

    return () => {
      isDisposed = true;
      connection.off("bookingChanged");
      Promise.resolve(startPromise)
        .catch(() => {})
        .finally(() => {
          if (connection.state !== signalR.HubConnectionState.Disconnected) {
            connection.stop().catch(() => {});
          }
        });
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
    } catch {
      toast.error("Failed to cancel booking");
      return false;
    }
  };

  return { bookings, loading, errors, addBooking, editBooking, deleteBooking };
}