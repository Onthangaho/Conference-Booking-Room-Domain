"use client";

import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";

export function BookingDetailPageClient({ bookingId }) {
  const [booking, setBooking] = useState(null);
  const [loading, setLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);

  useEffect(() => {
    let mounted = true;

    const loadBooking = async () => {
      try {
        // Try employee endpoint first, then admin endpoint.
        let data;
        try {
          data = await apiClient.get("/Bookings/mine");
        } catch {
          data = await apiClient.get("/Bookings/all");
        }

        if (!mounted) return;

        const match = (Array.isArray(data) ? data : []).find(
          (item) => String(item.id) === String(bookingId)
        );

        if (!match) {
          setNotFound(true);
          return;
        }

        setBooking(match);
      } catch {
        if (mounted) {
          setNotFound(true);
        }
      } finally {
        if (mounted) {
          setLoading(false);
        }
      }
    };

    loadBooking();

    return () => {
      mounted = false;
    };
  }, [bookingId]);

  if (loading) return <main className="container">Loading booking details...</main>;
  if (notFound || !booking) return <main className="container">Booking not found.</main>;

  return (
    <main className="container">
      <h2>Booking Detail</h2>
      <div className="booking-card">
        <p><strong>ID:</strong> {booking.id}</p>
        <p><strong>Room:</strong> {booking.roomName}</p>
        <p><strong>Type:</strong> {booking.roomType}</p>
        <p><strong>Start:</strong> {new Date(booking.start).toLocaleString()}</p>
        <p><strong>End:</strong> {new Date(booking.endTime).toLocaleString()}</p>
        <p><strong>Status:</strong> {booking.status}</p>
        <p><strong>Created By:</strong> {booking.createdBy}</p>
      </div>
    </main>
  );
}
