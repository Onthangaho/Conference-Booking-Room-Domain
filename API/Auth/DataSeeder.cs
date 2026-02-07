using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static async Task SeedRolesAndUserAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        string[] roles = { "Admin", "User", "Receptionist", "FacilitiesManager" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed users so that we can test the login functionality
        var users = new[]
        {
            new { Username = "employee1", Role = "Employee", Password = "Emp@12345" },
            new { Username = "employee2", Role = "Employee", Password = "Emp@67890" }, // extra employee
            new { Username = "admin1", Role = "Admin", Password = "Adm@12345" },
            new { Username = "reception1", Role = "Receptionist", Password = "Rec@12345" },
            new { Username = "facilities1", Role = "FacilitiesManager", Password = "Fac@12345" }

        };

       // Create users and assign roles if they don't already exist in the database. This allows us to have some initial data for testing the authentication and role-based authorization features of our application.
        foreach(var user in users)
        {
            if (await userManager.FindByNameAsync(user.Username) == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = user.Username,
                    Email = $"{user.Username}@example.com"
                };

                await userManager.CreateAsync(newUser, user.Password);
                await userManager.AddToRoleAsync(newUser, user.Role);
                
            }
        }


    }
}