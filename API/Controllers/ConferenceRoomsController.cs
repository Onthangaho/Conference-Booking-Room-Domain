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
            var response = conferenceRooms.Select(r => new ConferenceRoomResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                RoomType = r.RoomType.ToString(),
                Capacity = r.Capacity
            });

            return Ok(response);
        }
                // GET: api/conferenceroom/{id}
        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _bookingManager.GetRooms().FirstOrDefault(r => r.Id == id);

            if (room == null)
                throw new ConferenceRoomNotFoundException(id);

            var response = new ConferenceRoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                RoomType = room.RoomType.ToString(),
                Capacity = room.Capacity
            };
            return Ok(response);
        }

        
    }
}