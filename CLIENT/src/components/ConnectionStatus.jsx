import { useEffect, useState } from "react";

export default function ConnectionStatus() {
  const [status, setStatus] = useState("Checking...");

  useEffect(() => {
    const checkConnection = async () => {
      try {
        const res = await fetch(`${import.meta.env.VITE_API_BASE_URL}/Bookings/all`, {
          headers: { Authorization: `Bearer ${localStorage.getItem("token")}` }
        });
     // Add artificial delay before updating status
        setTimeout(() => {
          if (res.ok) {
            setStatus("🟢 Connected");
          } else {
            setStatus("🔴 Backend Offline");
          }
        }, 1000); // 1 second delay
      } catch (err) {
        setTimeout(() => {
          setStatus("🔴 Backend Offline");
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