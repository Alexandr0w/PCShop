using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PCShop.Data.Models;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Seeding
{
    public static class IdentityDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRolesAsync(roleManager);
            await SeedUserAsync(userManager, AdminUserName, AdminPassword, AdminRoleName, "Admin User", "123 Admin Street", 
                "Admin City", "0000");

            await SeedUserAsync(userManager, ManagerUserName, ManagerPassword, ManagerRoleName, "Manager User", "456 Manager Blvd", 
                "Manager Town", "1111");

            await AssignUserRoleToUsersWithoutRolesAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = { AdminRoleName, ManagerRoleName, UserRoleName };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string role,
            string fullName,
            string address,
            string city,
            string postalCode)
        {
            ApplicationUser? existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName,
                    Address = address,
                    City = city,
                    PostalCode = postalCode
                };

                IdentityResult createResult = await userManager.CreateAsync(user, password);

                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    string errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"Failed to create user {email}: {errors}");
                }
            }
            else
            {
                var roles = await userManager.GetRolesAsync(existingUser);
                if (!roles.Contains(role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }
            }
        }

        private static async Task AssignUserRoleToUsersWithoutRolesAsync(UserManager<ApplicationUser> userManager)
        {
            var users = userManager.Users;

            foreach (ApplicationUser user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles == null || roles.Count == 0)
                {
                    await userManager.AddToRoleAsync(user, UserRoleName);
                }
            }
        }
    }
}
