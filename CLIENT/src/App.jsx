import Button from "./components/Button";
import Footer from "./components/Footer";
import BookingCard from "./components/BookingCard";
import { bookings } from "./data/mockData";
import "./styles/main.css";
import Navbar from "./components/Navbar";

function App() {
  

  return (
    <>

      <Navbar />

      <main className="container">

        <h1>Upcoming Bookings</h1>
        
        <div className="bookings-list">
          {bookings.map((booking) => (
            <BookingCard 
              key={booking.id}
              roomName={booking.roomName}
              roomType={booking.roomType}
              Location={booking.Location}
              date={booking.date}
              startTime={booking.startTime}
              endTime={booking.endTime}
              userName={booking.userName}
            />
          ))}
        </div>
        
      </main>
    </>
  )
}

export default App
