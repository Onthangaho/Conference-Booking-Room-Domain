using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBookingRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingManager _bookingManager;
        private readonly ConferenceBookingDbContext _dbContext;

        public BookingsController(BookingManager bookingManager, ConferenceBookingDbContext dbContext)
        {
            _bookingManager = bookingManager;
            _dbContext = dbContext;
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
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                CancelledAt = b.CancelledAt

            }).ToList();
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

            var room = await _bookingManager.GetRoomById(dtoBookingRequest.RoomId);
            ;
            if (room == null)
            {
                throw new ConferenceRoomNotFoundException(dtoBookingRequest.RoomId);
            }

            if (!room.IsActive)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "ROOM_UNAVAILABLE",
                    Message = $"The room '{room.Name}' is currently unavailable for booking.",
                    Category = "BusinessRuleViolation"
                });
            }
            var bookinRequest = new BookingRequest(room, dtoBookingRequest.Start, dtoBookingRequest.EndTime);
            var booking = await _bookingManager.CreateBooking(bookinRequest);
            string createdBy = User.IsInRole("Receptionist") ? "Receptionist"
            : User.Identity?.Name ?? "Unknown User";


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
        [Authorize(Roles = "Employee,Receptionist,Admin")] // Only authenticated users with Employee, Receptionist, or Admin roles can access this endpoint
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
            var booking = await _bookingManager.GetBookingById(dto.Id);
            if (booking == null)
            {
                throw new BookingNotFoundException(dto.Id);
            }

            var success = await _bookingManager.CancelBooking(dto.Id);

            if (!success)
            {
                throw new BookingNotFoundException(dto.Id);


            }
            var bookingResponse = new BookingResponseDto
            {
                Id = booking.Id,
                RoomName = booking.Room.Name,
                RoomType = booking.Room.RoomType.ToString(),
                Capacity = booking.Room.Capacity,
                Start = booking.Start,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt,
                CancelledAt = booking.CancelledAt
            };
            return Ok(bookingResponse);

        }
        // This endpoint allows an admin to delete a booking, but only if it has already been cancelled. This is a common business rule to prevent accidental deletion of active bookings and to maintain a record of past bookings that were cancelled.
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only authenticated users with Admin role can access this endpoint
        public async Task<IActionResult> DeleteBooking(int id)
        {

            var booking = await _bookingManager.GetBookingById(id);
            if (booking == null)
            {
                throw new BookingNotFoundException(id);
            }

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

        [HttpGet("by-room/{roomId}")]
        // [Authorize(Roles = "Admin,Employee,Receptionist")]
        public async Task<IActionResult> GetBookingsByRoom(int roomId)
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Room)
                .AsNoTracking()
                .Where(b => b.Room.Id == roomId)
                .Select(b => new BookingSummaryDto
                {
                    Id = b.Id,
                    Room = b.Room.Name,
                    Location = b.Room.Location,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })
                .ToListAsync();

            if (!bookings.Any())
            {
                return NotFound(new ErrorResponseDto
                {
                    ErrorCode = "NO_BOOKINGS_FOUND",
                    Message = $"No bookings found for room ID '{roomId}'.",
                    Category = "NotFound"
                });
            }

            return Ok(bookings);
        }

    

        [HttpGet("by-location/{location}")]
        //[Authorize(Roles = "Admin,Employee,Receptionist")]
        public async Task<IActionResult> GetBookingsByLocation(string location)
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Room)
                .AsNoTracking()
                .Where(b => b.Room.Location == location)
                .Select(b => new BookingSummaryDto
                {
                    Id = b.Id,
                    Room = b.Room.Name,
                    Location = b.Room.Location,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })
                .ToListAsync();
                if (!bookings.Any())
                {
                    return NotFound(new ErrorResponseDto
                    {
                        ErrorCode = "NO_BOOKINGS_FOUND",
                        Message = $"No bookings found for location '{location}'.",
                        Category = "NotFound"
                    });
                }

            return Ok(bookings);
        }
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetBookingsByStatus(BookingStatus status)
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Room)
                .AsNoTracking()
                .Where(b => b.Status == status)
                .Select(b => new BookingSummaryDto
                {
                    Id = b.Id,
                    Room = b.Room.Name,
                    Location = b.Room.Location,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })
                .ToListAsync();

                if (!bookings.Any())
                {
                    return NotFound(new ErrorResponseDto
                    {
                        ErrorCode = "NO_BOOKINGS_FOUND",
                        Message = $"No bookings found with status '{status}'.",
                        Category = "NotFound"
                    });
                }

            return Ok(bookings);
        }

           [HttpGet("sorted")]
        //[Authorize(Roles = "Admin,Employee,Receptionist")]
        public async Task<IActionResult> GetSortedBookings(string sortBy = "date")
        {
            var query = _dbContext.Bookings.Include(b => b.Room).AsNoTracking();

            query = sortBy switch
            {
                "room" => query.OrderBy(b => b.Room.Name),
                "created" => query.OrderBy(b => b.CreatedAt),
                _ => query.OrderBy(b => b.Start)
            };

            var results = await query
                .Select(b => new BookingSummaryDto
                {
                    Id = b.Id,
                    Room = b.Room.Name,
                    Location = b.Room.Location,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("search")]
        //[Authorize(Roles = "Admin,Employee,Receptionist")]
        public async Task<IActionResult> SearchBookings(
            [FromQuery] int? roomId,
            [FromQuery] string? location,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] bool? isActive,
            [FromQuery] string sortBy = "date",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _dbContext.Bookings
                .Include(b => b.Room)
                .AsNoTracking()
                .AsQueryable();

            // Filtering
            if (roomId.HasValue)
                query = query.Where(b => b.Room.Id == roomId.Value);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(b => b.Room.Location == location);

            if (startDate.HasValue && endDate.HasValue)
                query = query.Where(b => b.Start >= startDate.Value && b.EndTime <= endDate.Value);

            if (isActive.HasValue)
                query = query.Where(b => b.Room.IsActive == isActive.Value);

            // Sorting
            query = sortBy switch
            {
                "room" => query.OrderBy(b => b.Room.Name),
                "created" => query.OrderBy(b => b.CreatedAt),
                _ => query.OrderBy(b => b.Start)
            };

            // Pagination
            var totalCount = await query.CountAsync();

            var bookings = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingSummaryDto
                {
                    Id = b.Id,
                    Room= b.Room.Name,
                    Location = b.Room.Location,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString()
                })
                .ToListAsync();

            var response = new PagedResponseDto<BookingSummaryDto>
            {
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
                Results = bookings
            };

            return Ok(response);
        }





   
    }


}


