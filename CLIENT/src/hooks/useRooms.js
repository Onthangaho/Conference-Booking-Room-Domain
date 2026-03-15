import { useState, useEffect } from "react";
import { fetchRooms } from "../services/roomService";
import axios from "axios";

export function useRooms() {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const controller = new AbortController();

    // Fetch rooms if
    const load = async () => {
      try {
        const data = await fetchRooms(controller.signal);
        setRooms(Array.isArray(data) ? data : []);
        setError("");
      } catch (err) {
        if (axios.isCancel(err) || err?.code === "ERR_CANCELED") {
          // Silent cancel
          return;
        }
        setError("Failed to load conference rooms.");
      } finally {
        setLoading(false);
      }
    };

    load();
    return () => controller.abort();
  }, []);

  return { rooms, loading, error };
}