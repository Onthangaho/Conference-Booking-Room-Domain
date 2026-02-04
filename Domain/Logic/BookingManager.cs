using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using ConferenceBookingRoomDomain;

namespace ConferenceBookingRoomDomain
{
    public class BookingManager // Centralised business logic for managing bookings 
    {
        //Properties 
        private readonly List<ConferenceRoom> _rooms = new();
        private readonly List<Booking> _bookings = new();

        //methods
        public IReadOnlyList<Booking> GetAllBookings()
        {
            return _bookings.ToList();
        }

        public Booking CreateBooking(BookingRequest request)
        {

            //Guard Clauses
            if (request.Room == null)
            {
                throw new Exception("Room cannot be null");
            }

            if (request.Start >= request.EndTime)
            {
                throw new Exception("Start time must be before end time");
            }
            //it checks if the room is already booked for the requested time slot
            bool overlaps = _bookings.Any(b => b.Room == request.Room &&
            b.Status == BookingStatus.Confirmed &&
            request.Start < b.EndTime && request.EndTime > b.Start);

            if (overlaps)
            {
              throw new BookingConflictException ();
            }
            Booking booking = new Booking(request.Room, request.Start,request.EndTime);

            booking.Confirm();
            _bookings.Add(booking);

            return booking;
        }
       
      



        
        public IReadOnlyList<ConferenceRoom> GetRooms()
        {
            return _rooms.AsReadOnly();
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
        public Dictionary<BookingStatus, List<Booking>> GroupBookingsByStatus()
        {
            var grouped = new Dictionary<BookingStatus, List<Booking>>();

            foreach (var booking in _bookings)
            {
                if (!grouped.ContainsKey(booking.Status))
                {
                    grouped[booking.Status] = new List<Booking>();
                }

                grouped[booking.Status].Add(booking);
            }

            return grouped;
        }

       



    }
}
