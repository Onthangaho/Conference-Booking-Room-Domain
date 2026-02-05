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
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "The booking request is invalid. Please check the provided data."
                });
            }
            try
            {
                 var room = _bookingManager.GetRooms().FirstOrDefault(r => r.Id == dtoBookingRequest.RoomId);
                 if (room == null)
                 {
                     return BadRequest(new ErrorResponseDto
                     {
                         ErrorCode = "ROOM_NOT_FOUND",
                         Message = $"Room with ID {dtoBookingRequest.RoomId} does not exist."
                     });
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
                return Conflict(new ErrorResponseDto
                {
                    ErrorCode = "BOOKING_CONFLICT",
                    Message = "The requested booking time conflicts with an existing booking."
                });
            }
            catch (DomainRuleViolationException ex)
            {
                return UnprocessableEntity(new ErrorResponseDto
                {
                    ErrorCode = "DOMAIN_RULE_VIOLATION",
                    Message = ex.Message
                });
                
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Message = "An unexpected error occurred while processing the booking request."
                });
            }
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "VALIDATION_ERROR",
                    Message = "The cancellation request is invalid. Please check the provided data."
                }); 
                
            }
            if (id != dto.Id)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "ID_MISMATCH",
                    Message = "The booking ID in the URL does not match the ID in the request body."
                });
            }
            var success = await _bookingManager.CancelBooking(dto.Id);

            if (!success)
            {
                return NotFound(new ErrorResponseDto
                {
                    ErrorCode = "BOOKING_NOT_FOUND",
                    Message = $"Booking with ID: {dto.Id} not found."
                });
                

            }
            return Ok(new ApiResponseDto
            {
                Message = $"Booking {dto.Id} has been cancelled."
            });
        
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {

            try
            {
                var success = await _bookingManager.DeleteBooking(id);
                if (!success)
                {
                    return NotFound(new ErrorResponseDto
                    {
                        ErrorCode = "BOOKING_NOT_FOUND",
                        Message = $"Booking with ID {id} not found."
                    });
                    
                }
    
                return Ok(new ApiResponseDto
                {
                    Message = $"Booking {id} has been deleted."
                });
            }
            catch (BookingException ex)
            {
                return Conflict(new ErrorResponseDto
                {
                    ErrorCode = "BOOKING_ERROR_CONFLICT",
                    Message = ex.Message
                });

                
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Message = "An unexpected error occurred while processing the delete request."
                });
            }
        }
    }


}