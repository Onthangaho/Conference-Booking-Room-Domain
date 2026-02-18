
import BookingCard from "./BookingCard";
import { bookings } from "../data/mockData";


function BookingList(){

    return(
        <div className="bookings-list">
            {bookings.map((booking) => (
                <BookingCard key={booking.id} booking={booking} />
            ))}
        </div>
    )
}

export default BookingList;