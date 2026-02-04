using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBookingRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceRoomsController : ControllerBase
    {
        private readonly BookingManager _bookingManager;

        public ConferenceRoomsController(BookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [HttpGet]
        public IActionResult GetAllConferenceRooms()
        {
            var conferenceRooms = _bookingManager.GetRooms();
            return Ok(conferenceRooms);
        }
                // GET: api/conferenceroom/{id}
        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _bookingManager.GetRooms().FirstOrDefault(r => r.Id == id);

            if (room == null)
                return NotFound($"Conference room with ID {id} not found.");

            return Ok(room);
        }

        
    }
}