export const bookings = [


    {
        id: 1,
        roomName: 'Conference Room A',
        roomType: 'Standard',
        location: 'Cape Town',
        date: '2026-02-23',
        startTime: '10:00',
        endTime: '11:00',
        userName: 'John Doe'
    }
    
]
// Function to get initial bookings, checking localStorage first and falling back to mock data if not found
export const getInitialBookings = () => {

    const storedBookings = localStorage.getItem('bookings');
    return storedBookings ? JSON.parse(storedBookings) : bookings;
}


export const resetBookings = () => {
    localStorage.removeItem('bookings');
}