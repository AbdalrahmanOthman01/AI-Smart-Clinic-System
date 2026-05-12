using AI_Smart_Clinic_System.Application.Constants;
using AI_Smart_Clinic_System.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AI_Smart_Clinic_System.Infrastructure.Persistence
{
    public class DBInitializer
    {
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(ILogger<DBInitializer> logger)
        {
            _logger = logger;
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Ensure database is created and migrations applied
            if (context.Database.GetPendingMigrations().Any())
            {
                await context.Database.MigrateAsync();
            }

            // 2. Seed Roles
            string[] roles = { Roles.Admin, Roles.Doctor, Roles.Patient, Roles.Assistant };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 3. Seed Admin Account
            const string adminPhone = "0123456789";
            const string adminEmail = "admin@clinic.local";
            const string adminPassword = "Admin@123456";

            if (await userManager.FindByNameAsync(adminPhone) is null)
            {
                var admin = new ApplicationUser
                {
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = adminPhone,
                    PhoneNumber = adminPhone,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
