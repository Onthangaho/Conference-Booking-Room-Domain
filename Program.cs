using System;

Console.WriteLine("Hello, World!");
Console.WriteLine("Welcome to the Conference Room Booking System!\n");


Console.Write("Enter your name: ");
string bookedBy = Console.ReadLine()!;
Console.Write("Enter booking time (yyyy-MM-dd HH:mm): ");
DateTime bookingTime = DateTime.Parse(Console.ReadLine()!);

Console.Write("Enter end time (yyyy-MM-dd HH:mm): ");
DateTime endTime = DateTime.Parse(Console.ReadLine()!);

Console.Write("Enter number of attendees: ");
int numberOfAttendees = int.Parse(Console.ReadLine()!);

RoomType suggestedRoomType = ConferenceRoom.GetRoomType(numberOfAttendees);
Console.WriteLine($"\nSuggested room type: {suggestedRoomType}");

BookingRequest request = new BookingRequest(bookedBy, bookingTime, endTime, numberOfAttendees);
ConferenceRoom room = new ConferenceRoom("Meeting Room A", suggestedRoomType);

// Process booking and handle exceptions if anything goes wrong it will be caught here and displayed to the user 
try
{
    Booking booking = new Booking(request.Name, request.BookingTime, request.EndTime, request.NumberOfAttendees);

    room.ValidateBooking(booking);
    booking.Confirmed();
    Console.WriteLine("Booking Details:\n");
    if (booking.Status == BookingStatus.Confirmed)
    {
        Console.WriteLine($"Booking successful for room: {room.Name}");
    }
    else
    {
        Console.WriteLine("Booking cancelled.");
    }

    Console.WriteLine($"Booked by: {booking.Name}");
    Console.WriteLine($"Room type: {room.RoomType} (with max capacity of {room.capacity} attendees)");
    Console.WriteLine($"Booking approved from {booking.BookingTime} to {booking.EndTime}");
    Console.WriteLine($"Status: {booking.Status}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"Booking failed: {ex.Message}");
}