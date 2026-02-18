import Heading from "./Heading";
import BookingList from "./BookingList";
import BookingForm from "./BookingForm";
import Button from "./Button";

function Dashboard({ bookings, addBooking, deleteBooking ,handleReset}) {


    return (

        <main className="container">

            <Heading title="Conference Room Bookings" />


            <p className="Counter">
                <strong>Total Bookings: {bookings.length}</strong>
            </p>

            <Heading title="Add a New Booking" />
            <BookingForm addBooking={addBooking} />

            <Button
                label="Reset Bookings to default Data"
                variant="danger"
                onClick={handleReset}
                 />

            <BookingList 
              bookings={bookings} 
              deleteBooking={deleteBooking} />
        </main>

            )
}

export default Dashboard;