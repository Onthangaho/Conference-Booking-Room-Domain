using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using ConferenceBookingRoomDomain;
using Conference_Booking_Room_Domain.Data;

namespace ConferenceBookingRoomDomain
{
    public class BookingManager // Centralised business logic for managing bookings 
    {
        //Properties 
        private readonly IBookingStore _bookingStore;
        private readonly IRoomStore _roomStore;



        public BookingManager(IBookingStore bookingStore, IRoomStore roomStore)
        {
            _bookingStore = bookingStore;
            _roomStore = roomStore;




        }


        // Helper method to generate unique booking IDs if not using a database auto-increment
        private int GenerateBookingId(IReadOnlyList<Booking> bookings)
        {
            if (bookings.Count == 0)
            {
                return 1;
            }
            return bookings.Max(b => b.Id) + 1;
        }
        //methods
        public async Task<IReadOnlyList<Booking>> GetAllBookings()
        {
            var bookings = await _bookingStore.LoadBookingAsync();
            return bookings.AsReadOnly();
        }

        public async Task<Booking> CreateBooking(BookingRequest request)
        {


            //Guard Clauses
            if (string.IsNullOrEmpty(request.UserId))
            {
                throw new Exception("User ID cannot be null or empty");
            }


            if (request.Start >= request.EndTime)
            {
                throw new InvalidBookingTimeException(request.Start, request.EndTime);
            }
            var room = await _roomStore.LoadRoomByIdAsync(request.RoomId);
            if (room == null)
            {
                throw new Exception($"Room with ID {request.RoomId} not found.");
            }
            //it checks if the room is already booked for the requested time slot
            var bookings = await _bookingStore.LoadBookingAsync();
            bool overlaps = bookings.Any(b =>
                b.RoomId == request.RoomId &&
                b.Status == BookingStatus.Confirmed &&
                request.Start < b.EndTime &&
                request.EndTime > b.Start);

            if (overlaps)
            {
                throw new BookingConflictException();
            }
            Booking booking = new Booking(
                           request.RoomId,
                           request.UserId,
                           request.Start,
                           request.EndTime
                         );
            booking.Confirm();


            await _bookingStore.SaveAsync(booking);

            return booking;
        }


        public async Task<bool> CancelBooking(int bookingId)
        {

            var bookings = await _bookingStore.LoadBookingAsync();
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
            {
                return false;
            }
            booking.Cancel();
            await _bookingStore.UpdateAsync(booking);
            return true;
        }


        public async Task<bool> DeleteBooking(int id)
        {

            var bookings = await _bookingStore.LoadBookingAsync();
            var booking = bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return false;
            }


            if (booking!.Status != BookingStatus.Cancelled)
            {
                throw new BookingDeleteConflictException(id);
            }
            booking.Delete();
            await _bookingStore.UpdateAsync(booking);

            return true;
        }



        public async Task<IReadOnlyList<ConferenceRoom>> GetRooms()
        {
            var rooms = await _roomStore.LoadRoomsAsync();
            return rooms.AsReadOnly();
        }

        public async Task<ConferenceRoom?> GetRoomById(int id)
        {
            return await _roomStore.LoadRoomByIdAsync(id);
        }
        public async Task<Booking?> GetBookingById(int id)
        {
            var booking = await _bookingStore.LoadBookingAsync();
            return booking.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
        }


        /*
         * Processes a booking request end-to-end.

         */
        /*
       public bool TryProcessBooking(int roomId, BookingRequest request)
       {

           ConferenceRoom? room =
               _rooms.FirstOrDefault(r => r.Id == roomId);

           if (room == null)
               throw new Exception("Selected room does not exist.");

           // Booking validates itself
           Booking booking = new Booking(room, request);

           bool approved = room.TryAddBooking(booking);

           _allBookings.Add(booking);
           return approved;
       }
       */



        /*
         * DEFENSIVE COPY
         */


        /*
         * GROUPING BOOKINGS BY STATUS

         */
        public async Task<Dictionary<BookingStatus, List<Booking>>> GroupBookingsByStatus()
        {
            var bookings = await _bookingStore.LoadBookingAsync();
            var grouped = bookings.GroupBy(b => b.Status)
               .ToDictionary(g => g.Key, g => g.ToList());
            return grouped;
        }





    }
}
