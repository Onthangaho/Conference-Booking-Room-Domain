import { useState, useEffect } from "react";
import {
  fetchAllBookings,
  addBookingToService,
  resetBookingsService
} from "./services/bookingService";
import Header from "./components/Header";
import Footer from "./components/Footer";
import "./styles/main.css";
import Navbar from "./components/Navbar";
import Dashboard from "./components/Dashboard";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  // State management for bookings, loading, and error handling
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Load bookings from service
  const loadBookings = async (controller) => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchAllBookings(controller.signal);
      setBookings(data);
      toast.success("Data Sync Successful!");
    } catch (err) {
      if (err.name !== "AbortError") {
        setError(err.message);
        toast.error(err.message);
      }
    } finally {
      setLoading(false);
    }
  };

  // Initial load with cleanup
  useEffect(() => {
    const controller = new AbortController();
    loadBookings(controller);
    return () => controller.abort();
  }, []);

  // Reset bookings to demo data
  const handleReset = () => {
    const reset = resetBookingsService();
    setBookings(reset);
    toast.info("Bookings reset to default demo data");
  };

  // Add booking via service
  const addBooking = (newBooking) => {
    const updated = addBookingToService({
      ...newBooking,
      id: Date.now(), // ensure unique ID
    });
    setBookings(updated);
    toast.success("Booking added successfully!");
  };

  // Delete booking locally (optional: extend service to persist deletes)
  const deleteBooking = (id) => {
    const updated = bookings.filter((b) => b.id !== id);
    setBookings(updated);
    toast.success("Booking deleted successfully!");
  };

  return (
    <>
      <Navbar />
      <Header />

      {loading && bookings.length === 0 && <p>Loading bookings...</p>}
      {loading && bookings.length > 0 && <p>Refreshing data...</p>}

      {error && (
        <div className="error-state">
          <p>{error}</p>
          <button
            className="retry-button"
            onClick={() => loadBookings(new AbortController())}
          >
            Retry
          </button>

        </div>
      )}

      {!loading && !error && (
        <Dashboard
          bookings={bookings}
          addBooking={addBooking}
          deleteBooking={deleteBooking}
          handleReset={handleReset}
        />
      )}

      <Footer />
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </>
  );
}

export default App;