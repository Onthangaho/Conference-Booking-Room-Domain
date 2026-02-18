import Button from "./Button";

function BookingCard({booking}) {


    return (

        <div className="booking-card">
            <h3>{booking.roomName}</h3>
            <p><strong>Type:</strong> {booking.roomType}</p>
            <p><strong>Location:</strong> {booking.location}</p>
            <p><strong>Date:</strong> {booking.date}</p>
            <p><strong>Time:</strong> {booking.startTime} - {booking.endTime}</p>
            <p><strong>User:</strong> {booking.userName}</p>


            <div className="card-actions">

                <Button label="Edit" variant="primary"/>
                <Button label="Cancel" variant="danger"/>

            </div>
            
        </div>
    )

}
export default BookingCard;