using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
public class BookingManager
{
    /*
     * INTERNAL MUTABLE STATE
     */
    private readonly List<ConferenceRoom> _rooms = new();
    private readonly List<Booking> _allBookings = new();

    public BookingManager()
    {
        SeedData();
    }

    public void SeedData()
    {
        ConferenceRoom room1 = new(1001, "Small Room", RoomType.Small);
        ConferenceRoom room2 = new(1002, "Medium Room", RoomType.Medium);
        ConferenceRoom room3 = new(1003, "Large Room", RoomType.Large);

        _rooms.Add(room1);
        _rooms.Add(room2);
        _rooms.Add(room3);

        Booking seededBooking = new Booking(room1,
            new BookingRequest("Alice", DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 4));
        room1.TryAddBooking(seededBooking);
        _allBookings.Add(seededBooking);
    }



    /*
     * DEFENSIVE COPY
     */
    public IReadOnlyList<ConferenceRoom> GetRooms()
    {
        return _rooms.AsReadOnly();
    }

    /*
     * Processes a booking request end-to-end.
     
     */
    public bool TryProcessBooking(int roomId, BookingRequest request)
    {
        /*
         * LINQ AS A QUESTION:
            * "Find me the room with this ID."
         */
        ConferenceRoom? room =
            _rooms.FirstOrDefault(r => r.Id == roomId);

        if (room == null)
            throw new Exception("Selected room does not exist.");

        // Booking validates itself
        Booking booking = new Booking(room, request);

        bool approved = room.TryAddBooking(booking);

        _allBookings.Add(booking);
        return approved;
    }




    /*
     * DEFENSIVE COPY
     */
    public IReadOnlyList<Booking> GetAllBookings()
    {
        return _allBookings.AsReadOnly();
    }

    /*
     * GROUPING BOOKINGS BY STATUS
     
     */
    public Dictionary<BookingStatus, List<Booking>> GroupBookingsByStatus()
    {
        var grouped = new Dictionary<BookingStatus, List<Booking>>();

        foreach (var booking in _allBookings)
        {
            if (!grouped.ContainsKey(booking.Status))
            {
                grouped[booking.Status] = new List<Booking>();
            }

            grouped[booking.Status].Add(booking);
        }

        return grouped;
    }

    public async Task SaveBookingsAsync(string filePath)
    {
        try
        {
            // Take a snapshot to avoid concurrent modification issues
            var snapshot = _allBookings.ToList();

            // Serialize with indentation for readability
            string json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });

            // Save asynchronously
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            // Wrap in domain-specific exception
            throw new BookingException($"Failed to save bookings: {ex.Message}");
        }
    }

   public async Task LoadBookingsAsync(string filePath)
{
    try
    {
        if (!File.Exists(filePath))
            throw new BookingException("Booking file not found.");

        string json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter()); // enums as strings

        var loadedBookings = JsonSerializer.Deserialize<List<Booking>>(json, options) ?? new List<Booking>();

        // Reset manager state
        _allBookings.Clear();
        foreach (var room in _rooms)
        {
            room.ClearBookings(); // helper in ConferenceRoom
        }

        // Re‑attach loaded bookings to rooms without re‑confirming
        foreach (var booking in loadedBookings)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == booking.Room.Id);
            if (room != null)
            {
                room.AttachBooking(booking);   // 👈 use helper instead of TryAddBooking
                _allBookings.Add(booking);
            }
        }
    }
    catch (JsonException)
    {
        throw new BookingException("Booking file is corrupted and could not be loaded.");
    }
    catch (Exception ex)
    {
        throw new BookingException($"Failed to load bookings: {ex.Message}");
    }
}


}
