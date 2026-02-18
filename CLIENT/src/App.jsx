import Button from "./components/Button";
import Footer from "./components/Footer";
import BookingCard from "./components/BookingCard";
import { bookings } from "./data/mockData";
import "./styles/main.css";
import Navbar from "./components/Navbar";
import Heading from "./components/Heading";
import BookingList from "./components/BookingList";

function App() {
  

  return (
    <>

      <Navbar />

      <main className="container">

        <Heading title="Conference Room Bookings" />

        <BookingList />
       
        
      </main>

      <Footer />
    </>
  )
}

export default App
