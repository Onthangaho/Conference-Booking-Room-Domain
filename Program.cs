using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Conference_Booking_Room_Domain.Data;



namespace ConferenceBookingRoomDomain
{
    class Program
    {
        static async Task Main(string[] args)
        {
          //Load system data
          SeedData data = new SeedData();
          List<ConferenceRoom> rooms = data.SeedRooms();
          BookingManager bookingManager = new BookingManager();
          BookingFireStore store = new BookingFireStore("bookings.json");


          try
          {
            bookingManager.CreateBooking(new BookingRequest(rooms[0], DateTime.Now.AddHours(1), DateTime.Now.AddHours(2)));
            await store.SaveAsync(bookingManager.GetAllBookings());
          }
          catch (BookingConflictException ex)
          {
            
            Console.WriteLine($"Error creating booking: {ex.Message}");
          }

        }

        
    }
}

