using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



public class ConferenceBookingDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options)
    {
    }
   
    public DbSet<ConferenceRoom> ConferenceRooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional model configuration can go here

        //booking entity rules so that it can be used in the conference room entity
        modelBuilder.Entity<Booking>()
        .HasKey(b => b.Id);

        //Conference entity rules so that it can be used in the booking entity
        modelBuilder.Entity<ConferenceRoom>()
        .HasKey(c => c.Id);
    }

}
