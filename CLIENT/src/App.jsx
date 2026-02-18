import { useState, useEffect } from "react";
import Button from "./components/Button";
import Footer from "./components/Footer";
import BookingCard from "./components/BookingCard";
import {getInitialBookings, resetBookings } from "./data/mockData";
import "./styles/main.css";
import Navbar from "./components/Navbar";
import Heading from "./components/Heading";
import BookingList from "./components/BookingList";
import Dashboard from "./components/Dashboard";

function App() {
  
const [bookings, setBookings] = useState(getInitialBookings());

useEffect(() => {
  localStorage.setItem('bookings', JSON.stringify(bookings));
}, [bookings]);

const addBooking = (newBooking) => {
  setBookings([...bookings, newBooking]);
};

const deleteBooking = (id) => {
  setBookings(prev=> prev.filter(booking => booking.id !== id));
};

const handleReset = () => {
  resetBookings();
  setBookings(getInitialBookings());
}
  return (
    <>

      <Navbar />

      <Dashboard 
        bookings={bookings} 
        addBooking={addBooking} 
        deleteBooking={deleteBooking} 
        handleReset={handleReset} />

      <Footer />
    </>
  )
}

export default App
