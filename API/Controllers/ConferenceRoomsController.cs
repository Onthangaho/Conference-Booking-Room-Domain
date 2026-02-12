using System.Threading.Tasks;
using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceRoomsController : ControllerBase
    {
        private readonly BookingManager _bookingManager;
        private readonly ConferenceBookingDbContext _dbContext;

        public ConferenceRoomsController(BookingManager bookingManager, ConferenceBookingDbContext dbContext)
        {
            _bookingManager = bookingManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConferenceRooms()
        {
            var conferenceRooms = await _bookingManager.GetRooms();
            var response = conferenceRooms.Select(r => new ConferenceRoomResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                RoomType = r.RoomType.ToString(),
                Capacity = r.Capacity,
                Location = r.Location,
                IsActive = r.IsActive,

            })
            .ToList();

            return Ok(response);
        }
        // GET: api/conferenceroom/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _bookingManager.GetRoomById(id);

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
        [HttpGet("by-active/{isActive}")]
        public async Task<IActionResult> GetRoomsByActiveStatus(bool isActive)
        {
            var rooms = await _dbContext.ConferenceRooms
                .AsNoTracking()
                .Where(r => r.IsActive == isActive)
                .Select(r => new ConferenceRoomResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Location = r.Location,
                    Capacity = r.Capacity,
                    RoomType = r.RoomType.ToString(),
                    IsActive = r.IsActive
                })
                .ToListAsync();
                if (rooms == null || rooms.Count == 0)
                {
                    return NotFound($"No conference rooms found with IsActive status: {(isActive ? "Active" : "Inactive")}");
                }

            return Ok(rooms);
        }


    }
}