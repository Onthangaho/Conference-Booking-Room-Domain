
import BookingCard from "./BookingCard";



function BookingList({ bookings, deleteBooking }) {

    return(
        <div className="bookings-list">
            {bookings.map((booking) => (
                <BookingCard key={booking.id} booking={booking} deleteBooking={deleteBooking}/> 
            ))}
        </div>
    )
}

export default BookingList;