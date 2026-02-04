using System.Text.Json;
using ConferenceBookingRoomDomain;
public class BookingFireStore : IBookingStore
{

    private readonly string _filePath;

    public BookingFireStore(string filePath)
    {

        _filePath = filePath;
    }

    public async Task SaveAsync(IEnumerable<Booking> bookings)
    {
     
     string json = JsonSerializer.Serialize(bookings);
     await File.WriteAllTextAsync(_filePath,json);

    }

    public async Task<List<Booking>> LoadBookingAsync()
    {

        if (!File.Exists(_filePath))
        {
            return new List<Booking>();
        }

        string json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Booking>>(json) ?? new List<Booking>();
        
    }
}