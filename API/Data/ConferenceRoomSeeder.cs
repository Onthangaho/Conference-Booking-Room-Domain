using Microsoft.EntityFrameworkCore;

public static class ConferenceRoomSeeder
{
    public static async Task SeedRoomsAsync(ConferenceBookingDbContext context)
    {
        var rooms = new List<ConferenceRoom>
        {
            new ConferenceRoom { Name = "Room A", Capacity = 10, RoomType = RoomType.Standard , Location = "Cape Town", IsActive = true},
                new ConferenceRoom { Name = "Room B", Capacity = 20, RoomType = RoomType.BoardRoom, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room C", Capacity = 15, RoomType = RoomType.Training, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Name = "Room D", Capacity = 25, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room E", Capacity = 30, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Name = "Room F", Capacity = 10, RoomType = RoomType.Training, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room G", Capacity = 20, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room H", Capacity = 15, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Name = "Room I", Capacity = 13, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Name = "Room J", Capacity = 20, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room K", Capacity = 10, RoomType = RoomType.BoardRoom, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room L", Capacity = 5, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Name = "Room M", Capacity = 12, RoomType = RoomType.Standard, Location = "Bloemfontein", IsActive = true },
                new ConferenceRoom { Name = "Room N", Capacity = 15, RoomType = RoomType.BoardRoom, Location = "Cape Town", IsActive = false },
                new ConferenceRoom { Name = "Room O", Capacity = 12, RoomType = RoomType.Training, Location = "Cape Town", IsActive = true },
                new ConferenceRoom { Name = "Room P", Capacity = 30, RoomType = RoomType.Standard, Location = "Cape Town", IsActive = true }
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
