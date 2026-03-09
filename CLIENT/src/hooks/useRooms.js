import { useState, useEffect } from "react";
import { fetchRooms } from "../services/roomService";
import axios from "axios";

export function useRooms() {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const controller = new AbortController();

    const load = async () => {
      try {
        const data = await fetchRooms(controller.signal);
        setRooms(Array.isArray(data) ? data : []);
      } catch (err) {
        if (axios.isCancel(err) || err?.code === "ERR_CANCELED") {
          // Silent cancel
          return;
        }
        console.error("Failed to load rooms");
      } finally {
        setLoading(false);
      }
    };

    load();
    return () => controller.abort();
  }, []);

  return { rooms, loading };
}