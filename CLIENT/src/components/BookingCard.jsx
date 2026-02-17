import Button from "./Button";

function BookingCard({roomName,roomType,Location,date,startTime,endTime,userName}) {


    return (

        <div className="booking-card">
            <h3>{roomName}</h3>
            <p><strong>Type:</strong> {roomType}</p>
            <p><strong>Location:</strong> {Location}</p>
            <p><strong>Date:</strong> {date}</p>
            <p><strong>Time:</strong> {startTime} - {endTime}</p>
            <p><strong>User:</strong> {userName}</p>


            <div className="card-actions">

                <Button label="Edit" variant="primary"/>
                <Button label="Cancel" variant="danger"/>

            </div>
            
        </div>
    )

}
export default BookingCard;