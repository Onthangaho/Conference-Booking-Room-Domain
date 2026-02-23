using System.Text.Json.Serialization;
using Conference_Booking_Room_Domain.Data;
using ConferenceBookingRoomDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ConferenceBookingRoomDomain.Domain;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ConferenceBookingAPI", Version = "v1" });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    });
    // Require JWT authentication for all endpoints in Swagger
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// Register Booking persistence using a safe, known file location
/*builder.Services.AddSingleton<IBookingStore>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();

    // Store file in the root of the API project
    string filePath = Path.Combine(env.ContentRootPath, "bookings.json");

    return new BookingFireStore(filePath);
});
*/
builder.Services.AddDbContext<ConferenceBookingDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services so we can manage users and roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ConferenceBookingDbContext>()
    .AddDefaultTokenProviders();
// Add authentication with JWT Bearer tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))

    };
});
// Register the BookingManager as a scoped service so it can be injected into controllers

builder.Services.AddScoped<IRoomStore, RoomStore>();
builder.Services.AddScoped<IBookingStore, EFBookingStore>();
//builder.Services.AddSingleton<SeedData>();
builder.Services.AddScoped<BookingManager>();


builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


var app = builder.Build();

// Seed initial data for users and roles
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConferenceBookingDbContext>();
    await dbContext.Database.EnsureCreatedAsync(); // Ensure the database is created before seeding
    await DataSeeder.SeedRolesAndUserAsync(scope.ServiceProvider);
    //await ConferenceRoomSeeder.SeedRoomsAsync(dbContext);
}
app.UseAuthentication();
app.UseAuthorization();
// Add custom exception handling middleware to catch and handle exceptions globally in a consistent way, providing better error responses to clients and improving the overall robustness of the API.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();


