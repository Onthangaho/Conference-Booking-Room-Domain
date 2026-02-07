using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{   
public class ConferenceBookingDbContext : IdentityDbContext<ApplicationUser>
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options)
    {
    }

}
}