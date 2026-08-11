using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Api
{
    public class SeedForm
    {
        public static async Task SeedAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("SystemAdministrator"))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "SystemAdministrator"
                });
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "HRAdministrator"
                });
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Manager"
                });
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Supervisor"
                });
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Employee"
                });

            }

            var user = await userManager.FindByNameAsync("SystemAdministrator");
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "systemAdministrator@gmail.com",
                    Email = "systemAdministrator@gmail.com"
                };
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "SystemAdministrator");
            }

        }
    }
}
