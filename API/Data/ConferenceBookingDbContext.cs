using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConferenceBookingRoomDomain.Domain;



public class ConferenceBookingDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ConferenceBookingDbContext(DbContextOptions<ConferenceBookingDbContext> options) : base(options)
    {
    }

    public DbSet<ConferenceRoom> ConferenceRooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Session> Sessions { get; set; }
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
       

       
       
        modelBuilder.Entity<Session>()
        .HasData(
            new Session
            {
                Id = 1,
                Title = "Daily Standup",
                Capacity = 10,
                Start = DateTime.UtcNow.AddDays(2),
                End = DateTime.UtcNow.AddDays(2).AddHours(1)
            }
        );
        // Configure the relationship between Booking and ApplicationUser
        modelBuilder.Entity<Booking>()
        .HasOne(b => b.User)
        .WithMany()
        .HasForeignKey(b => b.UserId)
        .OnDelete(DeleteBehavior.Restrict);
        // Configure the relationship between Booking and ConferenceRoom
        modelBuilder.Entity<Booking>()
        .HasOne(b => b.Room)
        .WithMany(r => r.Bookings)
        .HasForeignKey(b => b.RoomId)
        .OnDelete(DeleteBehavior.Restrict);
        
        // Set default value for IsActive property in ConferenceRoom
        modelBuilder.Entity<ConferenceRoom>()
        .Property(c => c.IsActive)
        .HasDefaultValue(true);

        modelBuilder.Entity<Booking>()
        .Property(b => b.Status)
        .HasDefaultValue(BookingStatus.Pending);

        // Default value for CreatedAt
        modelBuilder.Entity<Booking>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");




    }

}
