public static class ConferenceRoomSeeder
{
    public static void SeedRooms(ConferenceBookingDbContext context)
    {
        var rooms = new List<ConferenceRoom>
        {
            new ConferenceRoom { Name = "Room A", Capacity = 10, RoomType = RoomType.Standard },
                new ConferenceRoom { Name = "Room B", Capacity = 20, RoomType = RoomType.BoardRoom },
                new ConferenceRoom { Name = "Room C", Capacity = 15, RoomType = RoomType.Training },
                new ConferenceRoom { Name = "Room D", Capacity = 25, RoomType = RoomType.Standard },
                new ConferenceRoom { Name = "Room E", Capacity = 30, RoomType = RoomType.BoardRoom },
                new ConferenceRoom { Name = "Room F", Capacity = 10, RoomType = RoomType.Training },
                new ConferenceRoom { Name = "Room G", Capacity = 20, RoomType = RoomType.Standard },
                new ConferenceRoom { Name = "Room H", Capacity = 15, RoomType = RoomType.BoardRoom },
                new ConferenceRoom { Name = "Room I", Capacity = 13, RoomType = RoomType.Training },
                new ConferenceRoom { Name = "Room J", Capacity = 20, RoomType = RoomType.Standard },
                new ConferenceRoom { Name = "Room K", Capacity = 10, RoomType = RoomType.BoardRoom },
                new ConferenceRoom { Name = "Room L", Capacity = 5, RoomType = RoomType.Training },
                new ConferenceRoom { Name = "Room M", Capacity = 12, RoomType = RoomType.Standard },
                new ConferenceRoom { Name = "Room N", Capacity = 15, RoomType = RoomType.BoardRoom },
                new ConferenceRoom { Name = "Room O", Capacity = 12, RoomType = RoomType.Training },
                new ConferenceRoom { Name = "Room P", Capacity = 30, RoomType = RoomType.Standard }
            };
// Check if the rooms already exist in the database to avoid duplicates
            foreach (var room in rooms)
            {
                //if the room with the same name does not exist, add it to the database
                if (!context.ConferenceRooms.Any(r => r.Name == room.Name))
                {
                    context.ConferenceRooms.Add(room);
                }
            }

            context.SaveChanges();
        }
     }
