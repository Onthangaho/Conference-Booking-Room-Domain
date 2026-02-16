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
        modelBuilder.Entity<ConferenceRoom>()
        .HasData(
             new ConferenceRoom { Id = 1, Name = "Room A", Capacity = 10, RoomType = RoomType.Standard , Location = "Cape Town", IsActive = true},
                new ConferenceRoom { Id = 2, Name = "Room B", Capacity = 20, RoomType = RoomType.BoardRoom, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 3, Name = "Room C", Capacity = 15, RoomType = RoomType.Training, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Id = 4, Name = "Room D", Capacity = 25, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 5, Name = "Room E", Capacity = 30, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Id = 6, Name = "Room F", Capacity = 10, RoomType = RoomType.Training, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 7, Name = "Room G", Capacity = 20, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 8, Name = "Room H", Capacity = 15, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Id = 9, Name = "Room I", Capacity = 13, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Id = 10, Name = "Room J", Capacity = 20, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 11, Name = "Room K", Capacity = 10, RoomType = RoomType.BoardRoom, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 12, Name = "Room L", Capacity = 5, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Id = 13, Name = "Room M", Capacity = 12, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Id = 14, Name = "Room N", Capacity = 15, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Id = 15, Name = "Room O", Capacity = 12, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Id = 16, Name = "Room P", Capacity = 30, RoomType = RoomType.Standard, Location = "Cape Town", IsActive = true }
        );
        // Configure the relationship between Booking and ApplicationUser
        modelBuilder.Entity<Booking>()
        .HasOne(b => b.User)
        .WithMany()
        .HasForeignKey(b => b.UserId)
        //this is to prevent cascading deletes when a user is deleted, so that the booking records are not automatically deleted when a user is removed from the system.
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
