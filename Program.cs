using System;


Console.WriteLine("Welcome to the Conference Room Booking System!\n");

BookingManager manager = new BookingManager();

// Adding sample rooms with mock data
var room1 = new ConferenceRoom("Small Room", RoomType.Small);
var room2 = new ConferenceRoom("Medium Room", RoomType.Medium);
var room3 = new ConferenceRoom("Large Room", RoomType.Large);

manager.AddRoom(room1);
manager.AddRoom(room2);
manager.AddRoom(room3);

// Display available rooms
Console.WriteLine("\nAvailable Rooms:");
foreach (var room in manager.GetRooms())
{
    Console.WriteLine($"{room.Name} | Capacity: {room.Capacity} | ID: {room.Id}");
}

// Collect booking details from user

Console.Write("\nEnter Room ID: ");
Guid roomId = Guid.Parse(Console.ReadLine()!);

// Find the selected room
ConferenceRoom? selectedRoom = manager.GetRooms().FirstOrDefault(r => r.Id == roomId);
// check if it exists
if (selectedRoom == null)
{
    Console.WriteLine("The selected room does not exist.");
    return;
}
// Gather booking information
Console.Write("Enter your name: ");
string bookedBy = Console.ReadLine()!;
Console.Write("Enter booking time (yyyy-MM-dd HH:mm): ");
DateTime bookingTime = DateTime.Parse(Console.ReadLine()!);

Console.Write("Enter end time (yyyy-MM-dd HH:mm): ");
DateTime endTime = DateTime.Parse(Console.ReadLine()!);

Console.Write("Enter number of attendees: ");
int numberOfAttendees = int.Parse(Console.ReadLine()!);

// check availability of the room for the requested time slot
if (!selectedRoom.IsAvailable(bookingTime, endTime))
{
    Console.WriteLine("The room is not available for the selected time slot.");
    return;
}

// Create booking request and process booking
BookingRequest request = new BookingRequest(bookedBy, bookingTime, endTime, numberOfAttendees);






// Process booking and handle exceptions if anything goes wrong it will be caught here and displayed to the user 
try
{
    bool approved = manager.TryProcessBooking(roomId, request);
    if (approved)
    {
        Console.WriteLine("\n--- Booking Details ---");
        Console.WriteLine($"Room: {selectedRoom.Name}");
        Console.WriteLine($"Booked By: {request.Name}");
        Console.WriteLine($"Booking Time: {request.BookingTime}");
        Console.WriteLine($"End Time: {request.EndTime}");
        Console.WriteLine($"Number of Attendees: {request.NumberOfAttendees}");
        Console.WriteLine("Booking Status: Confirmed\n");
    }
    else
    {
        Console.WriteLine("Booking rejected due to overlap please try another time slot or book a different room.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Booking failed: {ex.Message}");
}

// Display all bookings grouped by status
Console.WriteLine("\n--- Bookings Grouped By Status ---");

var grouped = manager.GroupBookingsByStatus();

foreach (var status in grouped.Keys)
{
    Console.WriteLine($"\n{status}:");

    foreach (var booking in grouped[status])
    {
        Console.WriteLine($"- {booking.Name} ({booking.Room.Name})");
    }
}