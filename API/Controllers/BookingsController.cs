using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingManager _bookingManager;

        public BookingsController(BookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingManager.GetAllBookings();
            // Mapping to DTOs so we don't expose internal domain models directly to the API consumers
            var response = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomName = b.Room.Name,
                RoomType = b.Room.RoomType.ToString(),
                Capacity = b.Room.Capacity,
                Start = b.Start,
                EndTime = b.EndTime,
                Status = b.Status.ToString()

            });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dtoBookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                 var room = _bookingManager.GetRooms().FirstOrDefault(r => r.Id == dtoBookingRequest.RoomId);
                 if (room == null)
                 {
                     return BadRequest($"Room with ID {dtoBookingRequest.RoomId} does not exist.");
                 }
                 var bookinRequest = new BookingRequest(room, dtoBookingRequest.Start, dtoBookingRequest.EndTime);
                 var booking = await _bookingManager.CreateBooking(bookinRequest);
                 
                 // Map to Response DTO so we don't expose internal domain model directly
                 var bookingResponse = new BookingResponseDto
                 {
                     Id = booking.Id,
                     RoomName = booking.Room.Name,
                     RoomType = booking.Room.RoomType.ToString(),
                     Capacity = booking.Room.Capacity,
                     Start = booking.Start,
                     EndTime = booking.EndTime,
                     Status = booking.Status.ToString()
                 };
                return Ok(bookingResponse);
            }
            catch (BookingConflictException)
            {
                return Conflict("The requested time slot is already booked.");
            }
            catch (Exception ex)
            {
                UnprocessableEntity(ex.Message);
                return UnprocessableEntity(ex.Message);
            }
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var success = await _bookingManager.CancelBooking(id);

            if (!success)
            {
                return NotFound($"Booking with ID: {id} not found.");

            }
            return Ok($"Booking {id} has been cancelled.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {

            try
            {
                var success = await _bookingManager.DeleteBooking(id);
                if (!success)
                {
                    return NotFound($"Booking with ID {id} not found.");
                }

                return Ok($"Booking {id} has been deleted.");
            }
            catch (BookingException ex)
            {

                return Conflict(ex.Message);
            }
        }
    }


}