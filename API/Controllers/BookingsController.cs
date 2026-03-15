using System.Security.Claims;
using ConferenceBookingRoomAPI.Hubs;
using ConferenceBookingRoomDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace ConferenceBookingRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly BookingManager _bookingManager;
        private readonly ConferenceBookingDbContext _dbContext;
        private readonly IHubContext<BookingsHub> _bookingsHub;

        public BookingsController(BookingManager bookingManager, ConferenceBookingDbContext dbContext, IHubContext<BookingsHub> bookingsHub)
        {
            _bookingManager = bookingManager;
            _dbContext = dbContext;
            _bookingsHub = bookingsHub;
        }

        private async Task BroadcastBookingChangedAsync(string action, object payload, string? createdBy = null)
        {
            var tasks = new List<Task>
            {
                _bookingsHub.Clients.Group("role:admin").SendAsync("bookingChanged", action, payload)
            };

            if (!string.IsNullOrWhiteSpace(createdBy))
            {
                tasks.Add(_bookingsHub.Clients.Group($"user:{createdBy.ToLowerInvariant()}").SendAsync("bookingChanged", action, payload));
            }

            await Task.WhenAll(tasks);
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
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null)
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponseDto
            {
                Id = b.Id,
                RoomId = b.RoomId,
                RoomName = b.Room.Name,
                RoomType = b.Room.RoomType.ToString(),
                Capacity = b.Room.Capacity,
                Start = b.Start,
                EndTime = b.EndTime,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                CreatedBy = b.User != null ? b.User.UserName : "Unknown User",
                CancelledAt = b.CancelledAt.HasValue
                ? b.CancelledAt.Value.ToString("yyyy-MM-dd HH:mm")
                 : "Not Cancelled",
                IsCancelled = b.CancelledAt.HasValue

            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLowerInvariant();
                bookings = bookings
                    .Where(b =>
                        (b.RoomName?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.RoomType?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.CreatedBy?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.Status?.ToLowerInvariant().Contains(normalizedSearch) ?? false))
                    .ToList();
            }

            return Ok(bookings);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Employee,Receptionist")]
        public async Task<IActionResult> GetMine([FromQuery] string? searchTerm = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    ErrorCode = "USER_NOT_FOUND",
                    Message = "Unable to identify the current user.",
                    Category = "AuthenticationError"
                });
            }

            var bookings = await _dbContext.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .AsNoTracking()
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    RoomType = b.Room.RoomType.ToString(),
                    Capacity = b.Room.Capacity,
                    Start = b.Start,
                    EndTime = b.EndTime,
                    Status = b.Status.ToString(),
                    CreatedAt = b.CreatedAt,
                    CreatedBy = b.User != null ? b.User.UserName : "Unknown User",
                    CancelledAt = b.CancelledAt.HasValue
                        ? b.CancelledAt.Value.ToString("yyyy-MM-dd HH:mm")
                        : "Not Cancelled",
                    IsCancelled = b.CancelledAt.HasValue
                })
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLowerInvariant();
                bookings = bookings
                    .Where(b =>
                        (b.RoomName?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.RoomType?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.CreatedBy?.ToLowerInvariant().Contains(normalizedSearch) ?? false) ||
                        (b.Status?.ToLowerInvariant().Contains(normalizedSearch) ?? false))
                    .ToList();
            }

            return Ok(bookings);
        }

        [HttpPost]
        [Authorize(Roles = "Employee,Receptionist")] // Only authenticated users with Admin or User roles can access this endpoint
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dtoBookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var room = await _bookingManager.GetRoomById(dtoBookingRequest.RoomId);
            ;
            if (room == null)
            {
                throw new ConferenceRoomNotFoundException(dtoBookingRequest.RoomId);
            }

            if (!room.IsActive)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(CreateBookingDto.RoomId)] = new[] { $"The room '{room.Name}' is currently unavailable for booking." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "USER_NOT_FOUND",
                    Message = "Unable to identify the user making the booking request.",
                    Category = "AuthenticationError"
                });
            }
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorCode = "USER_NOT_FOUND",
                    Message = "The user making the booking request does not exist in the system.",
                    Category = "AuthenticationError"
                });
            }
            var bookinRequest = new BookingRequest(dtoBookingRequest.RoomId, userId, dtoBookingRequest.Start, dtoBookingRequest.EndTime);

            Booking booking;
            try
            {
                booking = await _bookingManager.CreateBooking(bookinRequest);
            }
            catch (InvalidBookingTimeException)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(CreateBookingDto.EndTime)] = new[] { "End time must be after start time." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (BookingConflictException)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(CreateBookingDto.RoomId)] = new[] { $"{room.Name} is already occupied in the selected time range." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var savedBooking = await _dbContext.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == booking.Id);

            if (savedBooking == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    ErrorCode = "BOOKING_NOT_FOUND",
                    Message = $"Booking with ID '{booking.Id}' was not found after creation.",
                    Category = "NotFound"
                });
            }



            // Map to Response DTO so we don't expose internal domain model directly
            var bookingResponse = new BookingResponseDto
            {
                Id = booking.Id,
                RoomId = savedBooking.RoomId,
                RoomName = savedBooking.Room.Name,
                RoomType = savedBooking.Room.RoomType.ToString(),
                Capacity = savedBooking.Room.Capacity,
                Start = savedBooking.Start,
                EndTime = savedBooking.EndTime,
                Status = savedBooking.Status.ToString(),
                CreatedAt = savedBooking.CreatedAt,
                CreatedBy = User.Identity?.Name ?? "Unknown User",
            };

            await BroadcastBookingChangedAsync("created", bookingResponse, bookingResponse.CreatedBy);
            return Ok(bookingResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee,Receptionist")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    ErrorCode = "USER_NOT_FOUND",
                    Message = "Unable to identify the current user.",
                    Category = "AuthenticationError"
                });
            }

            var booking = await _dbContext.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (booking == null)
            {
                throw new BookingNotFoundException(id);
            }

            if (booking.UserId != userId)
            {
                return Forbid();
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    ["status"] = new[] { "Cancelled bookings cannot be edited." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }

            if (dto.Start >= dto.EndTime)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(UpdateBookingDto.EndTime)] = new[] { "End time must be after start time." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }

            if (dto.RoomId != booking.RoomId)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(UpdateBookingDto.RoomId)] = new[] { "Editing a booking cannot change its room." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var hasOverlap = await _dbContext.Bookings
                .AsNoTracking()
                .AnyAsync(b =>
                    b.Id != id &&
                    !b.IsDeleted &&
                    b.RoomId == dto.RoomId &&
                    b.Status == BookingStatus.Confirmed &&
                    dto.Start < b.EndTime &&
                    dto.EndTime > b.Start);

            if (hasOverlap)
            {
                return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    [nameof(UpdateBookingDto.RoomId)] = new[] { $"{booking.Room.Name} is already occupied in the selected time range." }
                })
                {
                    Status = StatusCodes.Status400BadRequest
                });
            }

            booking.Start = dto.Start;
            booking.EndTime = dto.EndTime;
            await _dbContext.SaveChangesAsync();

            var updatedResponse = new BookingResponseDto
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                RoomName = booking.Room.Name,
                RoomType = booking.Room.RoomType.ToString(),
                Capacity = booking.Room.Capacity,
                Start = booking.Start,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt,
                CreatedBy = booking.User != null ? booking.User.UserName : "Unknown User",
                CancelledAt = booking.CancelledAt.HasValue
                    ? booking.CancelledAt.Value.ToString("yyyy-MM-dd HH:mm")
                    : "Not Cancelled",
                IsCancelled = booking.CancelledAt.HasValue
            };

            await BroadcastBookingChangedAsync("updated", updatedResponse, updatedResponse.CreatedBy);

            return Ok(updatedResponse);
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
                RoomId = booking.RoomId,
                RoomName = booking.Room.Name,
                RoomType = booking.Room.RoomType.ToString(),
                Capacity = booking.Room.Capacity,
                Start = booking.Start,
                EndTime = booking.EndTime,
                Status = BookingStatus.Cancelled.ToString(),
                CreatedAt = booking.CreatedAt,
                CancelledAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
                CreatedBy = booking.User?.UserName ?? "Unknown User",
                IsCancelled = true

            };

            await BroadcastBookingChangedAsync("cancelled", bookingResponse, bookingResponse.CreatedBy);
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

            await BroadcastBookingChangedAsync("deleted", new { Id = id }, booking.User?.UserName);

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
                    Room = b.Room.Name,
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


