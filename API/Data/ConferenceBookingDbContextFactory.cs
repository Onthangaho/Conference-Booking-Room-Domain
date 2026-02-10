using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ConferenceBookingDbContextFactoryConferenceBookingDbContextFactory : IDesignTimeDbContextFactory<ConferenceBookingDbContext>
{
    public ConferenceBookingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<ConferenceBookingDbContext>();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

        return new ConferenceBookingDbContext(optionsBuilder.Options);
    }
}