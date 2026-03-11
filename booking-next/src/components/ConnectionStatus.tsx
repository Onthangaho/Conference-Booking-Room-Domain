import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";

export default function ConnectionStatus() {
  const [status, setStatus] = useState("Checking...");

  useEffect(() => {
    const checkConnection = async () => {
      try {
        await apiClient.get("/ConferenceRooms");
        setTimeout(() => {
          setStatus("🟢 Connected");
        }, 1000); // 1 second delay
      } catch (err) {
        const status = err?.response?.status;
        setTimeout(() => {
          setStatus(status === 401 || status === 403 ? "🟢 Connected" : "🔴 Backend Offline");
        }, 1000);
      }
    };


    checkConnection();

    // Optional: repeat every 30s with cleanup
    const intervalId = setInterval(checkConnection, 30000);
    return () => clearInterval(intervalId);
  }, []);

  return (
    <span style={{ fontWeight: "bold", color: status.includes("Connected") ? "green" : "red" }}>
      {status}
    </span>
  );
}