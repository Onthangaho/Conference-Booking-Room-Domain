using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ConferenceBookingRoomDomain.Domain;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase// This controller handles user authentication and JWT token generation
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username!);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password!))
        {
            return Unauthorized(new { Message = "Invalid username or password" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        return Ok(new
        {
            Token = token,
            Username = user.UserName,
            Roles = roles
        });
    }
}