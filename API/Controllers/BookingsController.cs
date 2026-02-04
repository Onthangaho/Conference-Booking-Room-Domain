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
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingManager.GetAllBookings();
            return Ok(bookings);
        }

     
    }
}