using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class ConferenceBookingDbContext : IdentityDbContext<ApplicationUser>
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options)
    {
    }

}
