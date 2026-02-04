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
            return Ok(bookings);
        }
      
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequest request)
        {
            try
            {
                var booking = await _bookingManager.CreateBooking(request);
                 return Ok(booking);
            }
            catch (BookingConflictException)
            {
                return Conflict("The requested time slot is already booked.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
    }

    
}