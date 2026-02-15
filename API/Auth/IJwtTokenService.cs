
 using ConferenceBookingRoomDomain.Domain;
public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}