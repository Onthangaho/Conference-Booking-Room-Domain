// Simulated in-memory booking data store
let bookingsStore = [
  {
    id: 1,
    roomName: "Conference Room A",
    roomType: "Standard",
    location: "Cape Town",
    date: "2026-02-24",
    startTime: "09:00",
    endTime: "11:00",
    userName: "Onthangaho Magoro"
  },
  {
    id: 2,
    roomName: "Conference Room B",
    roomType: "Boardroom",
    location: "Bloemfontein",
    date: "2026-02-25",
    startTime: "14:00",
    endTime: "16:00",
    userName: "Pfano Sibei"
  }
];

export async function fetchAllBookings(signal) {
  return new Promise((resolve, reject) => {
    const delay = Math.floor(Math.random() * (2500 - 500 + 1)) + 500;
    const timer = setTimeout(() => {
      if (signal?.aborted) {
        clearTimeout(timer);
        reject(new Error("Request aborted"));
        return;
      }
      if (Math.random() < 0.2) {
        reject(new Error("Server error: Failed to fetch bookings. Please try again."));
      } else {
        resolve([...bookingsStore]); // return current store
      }
    }, delay);
  });
}

export function addBookingToService(newBooking) {
  bookingsStore.push(newBooking);
  return [...bookingsStore];
}

export function resetBookingsService() {
  bookingsStore = [
    {
      id: Date.now(),
      roomName: "Conference Room A",
      roomType: "Training",
      location: "Cape Town",
      date: "2026-02-20",
      startTime: "09:00",
      endTime: "10:00",
      userName: "Demo User"
    }
  ];
  return [...bookingsStore];
}