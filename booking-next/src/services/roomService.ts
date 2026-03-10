import apiClient from "../api/apiClient";

// Fetch all active conference rooms
export async function fetchRooms(signal) {
  return await apiClient.get("/ConferenceRooms/by-active/true", { signal });
}