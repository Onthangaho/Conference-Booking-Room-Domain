using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPost("maintenance")]
        [Authorize(Roles = "FacilitiesManager")] // Only authenticated users with FacilitiesManager role can access this endpoint
        public IActionResult BookMaitenanceRoom([FromBody] MaintenanceBookingDto dto)
        {

            return Ok(new ApiResponseDto
            {
                Message = $"Room {dto.RoomId} has been booked for maintenance on {dto.Date.ToShortDateString()}."
            });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")] // Only authenticated users with Admin or User roles can access this endpoint
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
        [Authorize(Roles = "Employee,Receptionist")] // Only authenticated users with Admin or User roles can access this endpoint
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dtoBookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "The booking request is invalid. Please check the provided data.",
                    Category = "ValidationError"
                });
            }


            var room = _bookingManager.GetRooms().FirstOrDefault(r => r.Id == dtoBookingRequest.RoomId);
            if (room == null)
            {
                throw new ConferenceRoomNotFoundException(dtoBookingRequest.RoomId);
            }
            var bookinRequest = new BookingRequest(room, dtoBookingRequest.Start, dtoBookingRequest.EndTime);
            var booking = await _bookingManager.CreateBooking(bookinRequest);
            string createdBy;
            if(User.IsInRole("Receptionist"))
            {
                createdBy = "Receptionist";
            }
            else
            {
                createdBy = User.Identity?.Name ?? "Unknown User";
            }

            // Map to Response DTO so we don't expose internal domain model directly
            var bookingResponse = new BookingResponseDto
            {
                Id = booking.Id,
                RoomName = booking.Room.Name,
                RoomType = booking.Room.RoomType.ToString(),
                Capacity = booking.Room.Capacity,
                Start = booking.Start,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                CreatedBy = createdBy
            };
            return Ok(bookingResponse);
        }



        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "The cancellation request is invalid. Please check the provided data.",
                    Category = "ValidationError"
                });

            }
            if (id != dto.Id)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "ID_MISMATCH",
                    Message = "The booking ID in the URL does not match the ID in the request body.",
                    Category = "ValidationError"
                });
            }
            var success = await _bookingManager.CancelBooking(dto.Id);

            if (!success)
            {
                throw new BookingNotFoundException(dto.Id);


            }
            return Ok(new ApiResponseDto
            {
                Message = $"Booking {dto.Id} has been cancelled."
            });

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only authenticated users with Admin role can access this endpoint
        public async Task<IActionResult> DeleteBooking(int id)
        {


            var success = await _bookingManager.DeleteBooking(id);
            if (!success)
            {
                throw new BookingNotFoundException(id);

            }

            return Ok(new ApiResponseDto
            {
                Message = $"Booking {id} has been deleted."
            });

        }
    }


}


