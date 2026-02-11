

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : Controller
{
    

    private readonly ConferenceBookingDbContext _dbContext;
    public SessionsController(ConferenceBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]

    public IActionResult GetSessions()
    {
        var sessions = _dbContext.Sessions.ToList();
        var response = sessions.Select(s => new SessionResponseDto
        {
            Id = s.Id,
            Title = s.Title,
            Capacity = s.Capacity,
            Start = s.Start,
            End = s.End
        }).ToList();

        return Ok(response);
    }
}