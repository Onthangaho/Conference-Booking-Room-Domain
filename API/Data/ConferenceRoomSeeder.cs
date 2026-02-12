using Microsoft.EntityFrameworkCore;

public static class ConferenceRoomSeeder
{
    public static async Task SeedRoomsAsync(ConferenceBookingDbContext context)
    {
        var rooms = new List<ConferenceRoom>
        {
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
            };
// Check if the rooms already exist in the database to avoid duplicates
            foreach (var room in rooms)
            {
                //if the room with the same name does not exist, add it to the database
             bool roomExists = await context.ConferenceRooms.AnyAsync(r => r.Name == room.Name);
                if (!roomExists)
                {
                    context.ConferenceRooms.Add(room);
                }
            }

            await context.SaveChangesAsync  ();
        }
     }
