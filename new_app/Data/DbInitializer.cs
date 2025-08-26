using HotelReservationSystem.Models.Domain;
using HotelReservationSystem.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created
            context.Database.Migrate();

            // Seed roles
            if (!await roleManager.RoleExistsAsync(RoleName.CanManageHotels))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleName.CanManageHotels));
            }

            // Seed admin user
            if (!context.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@hotel.com",
                    Email = "admin@hotel.com",
                    EmailConfirmed = true,
                    Phone = "123456789"
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, RoleName.CanManageHotels);
            }

            // Seed countries
            if (!context.Countries.Any())
            {
                var countries = new List<Country>
                {
                    new Country { Name = "Bulgaria" },
                    new Country { Name = "Germany" },
                    new Country { Name = "France" },
                    new Country { Name = "Italy" },
                    new Country { Name = "Spain" },
                    new Country { Name = "Egypt" },
                    new Country { Name = "Poland" },
                    new Country { Name = "Greece" },
                    new Country { Name = "Turkey" },
                    new Country { Name = "Malta" }
                };

                context.Countries.AddRange(countries);
                await context.SaveChangesAsync();
            }
        }
    }
}