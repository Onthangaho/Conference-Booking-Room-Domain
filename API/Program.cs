using System.Text.Json.Serialization;
using Conference_Booking_Room_Domain.Data;
using ConferenceBookingRoomDomain;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen();
// Register Booking persistence using a safe, known file location
builder.Services.AddSingleton<IBookingStore>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();

    // Store file in the root of the API project
    string filePath = Path.Combine(env.ContentRootPath, "bookings.json");

    return new BookingFireStore(filePath);
});
builder.Services.AddSingleton<BookingManager>();

builder.Services.AddSingleton<SeedData>();  


var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();


