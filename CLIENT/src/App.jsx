import Navbar from "./components/Navbar";
import Header from "./components/Header";
import Dashboard from "./components/Dashboard";
import Footer from "./components/Footer";
import { ToastContainer, toast } from "react-toastify";
import { useBookings } from "./hooks/useBookings";
import { addBookingToService, deleteBookingService } from "./services/bookingService";

function App() {
  // useBookings hook handles fetching, loading, error state
  const { bookings, loading, error, setBookings } = useBookings();

  const addBooking = async (newBooking) => {
    try {
      const created = await addBookingToService(newBooking);
      setBookings((prev) => [...prev, created]);
      toast.success("Booking added successfully!");
    } catch (err) {
      toast.error(err.message);
    }
  };

  const deleteBooking = async (id) => {
    try {
      const cancelled = await deleteBookingService(id);
      setBookings((prev) =>
        prev.map((b) => (b.id === id ? { ...b, ...cancelled } : b))
      );
      toast.success("Booking cancelled successfully!");
    } catch (err) {
      toast.error(err.message);
    }
  };

  return (
    <>
      <Navbar />
      <Header /> {/* ✅ shows ConnectionStatus */}

      {loading && <p>Loading bookings...</p>}
      {error && <p style={{ color: "red" }}>Error: {error}</p>}

      {!loading && !error && (
        <Dashboard
          bookings={bookings}
          addBooking={addBooking}
          deleteBooking={deleteBooking}
        />
      )}

      <Footer />
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </>
  );
}

export default App;