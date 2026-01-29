using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;



namespace ConferenceBookingRoomDomain
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Conference Room Booking System!\n");

           Console.WriteLine("=== Welcome to the Conference Room Booking System! ===\n");

        BookingManager manager = new BookingManager();
        string filePath = "bookings.json"; // persistence file

        // Try to load existing bookings at startup
        try
        {
            await manager.LoadBookingsAsync(filePath);
            Console.WriteLine("Previous bookings loaded from file.");
        }
        catch (BookingException ex)
        {
            Console.WriteLine($"ℹStartup info: {ex.Message}");
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. View available rooms");
            Console.WriteLine("2. Book a room");
            Console.WriteLine("3. View bookings grouped by status");
            Console.WriteLine("4. Save bookings to file (async)");
            Console.WriteLine("5. Load bookings from file (async)");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine()!;
            switch (choice)
            {
                case "1":
                    DisplayRooms(manager);
                    break;

                case "2":
                    BookRoom(manager);
                    break;

                case "3":
                    DisplayGroupedBookings(manager);
                    break;

                case "4":
                    await SaveBookings(manager, filePath);
                    break;

                case "5":
                    await LoadBookings(manager, filePath);
                    break;

                case "6":
                    exit = true;
                    Console.WriteLine("Exiting... Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

        }

        static void DisplayRooms(BookingManager manager)
        {
            Console.WriteLine("\nAvailable Rooms:");
            foreach (var room in manager.GetRooms())
            {
                Console.WriteLine($"Room {room.Id} | {room.Name} | Capacity: {room.Capacity}");
            }
        }
        static void BookRoom(BookingManager manager)
        {
            Console.Write("\nEnter room number: ");
            if (!int.TryParse(Console.ReadLine(), out int roomId))
            {
                Console.WriteLine("Invalid room number.");
                return;
            }

            var selectedRoom = manager.GetRooms().FirstOrDefault(r => r.Id == roomId);
            if (selectedRoom == null)
            {
                Console.WriteLine("Room not found.");
                return;
            }

            Console.Write("Enter your name: ");
            string bookedBy = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(bookedBy))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            Console.Write("Enter booking time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime bookingTime))
            {
                Console.WriteLine("Invalid booking time.");
                return;
            }

            Console.Write("Enter end time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endTime))
            {
                Console.WriteLine("Invalid end time.");
                return;
            }

            Console.Write("Enter number of attendees: ");
            if (!int.TryParse(Console.ReadLine(), out int attendees) || attendees <= 0)
            {
                Console.WriteLine("Invalid number of attendees.");
                return;
            }

            if (!selectedRoom.IsAvailable(bookingTime, endTime))
            {
                Console.WriteLine("Room not available for that time slot.");
                return;
            }

            var request = new BookingRequest(bookedBy, bookingTime, endTime, attendees);

            try
            {
                bool approved = manager.TryProcessBooking(roomId, request);
                Console.WriteLine("\n--- Booking Result ---");
                Console.WriteLine(approved ? "Booking Confirmed" : "Booking Rejected (overlap or capacity issue)");
            }
            catch (BookingException ex)
            {
                Console.WriteLine($"Booking Error: {ex.Message}");
            }
        }

        static void DisplayGroupedBookings(BookingManager manager)
        {
            Console.WriteLine("\n--- Bookings Grouped By Status ---");
            var grouped = manager.GroupBookingsByStatus();

            if (grouped.Count == 0)
            {
                Console.WriteLine("No bookings yet.");
                return;
            }

            foreach (var status in grouped.Keys)
            {
                Console.WriteLine($"\n{status}:");
                foreach (var booking in grouped[status])
                {
                    Console.WriteLine($"- {booking.Name} ({booking.Room.Name}) from {booking.BookingTime} to {booking.EndTime}");
                }
            }
        }

        static async Task SaveBookings(BookingManager manager, string filePath)
        {
            try
            {
                await manager.SaveBookingsAsync(filePath);
                Console.WriteLine($"\nBookings saved to {filePath}");
            }
            catch (BookingException ex)
            {
                Console.WriteLine($"Save failed: {ex.Message}");
            }
        }

        static async Task LoadBookings(BookingManager manager, string filePath)
        {
            try
            {
                await manager.LoadBookingsAsync(filePath);
                Console.WriteLine("\nBookings reloaded from file.");

                var bookings = manager.GetAllBookings();
                if(bookings.Count == 0)
                {
                    Console.WriteLine("No bookings found in the file.");
                }
                else
                {
                    Console.WriteLine("--_ Loaded Bookings ---");
                    foreach (var booking in bookings)
                    {
                        Console.WriteLine($"- {booking.Name} ({booking.Room.Name}) from {booking.BookingTime} to {booking.EndTime} number of attendees: {booking.NumberOfAttendees} | Status: {booking.Status}");
                    }
                }
            }
            catch (BookingException ex)
            {
                Console.WriteLine($"Load failed: {ex.Message}");
            }
        }


    }
}

