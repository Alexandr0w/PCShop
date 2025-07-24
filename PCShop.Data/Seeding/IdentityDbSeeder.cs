using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCShop.Data.Models;
using PCShop.Data.Seeding.Interfaces;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Seeding
{
    public class IdentityDbSeeder : IIdentityDbSeeder
    {
        private readonly IServiceProvider _serviceProvider;

        public IdentityDbSeeder(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task SeedAsync()
        {
            RoleManager<IdentityRole<Guid>> roleManager = this._serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            UserManager<ApplicationUser> userManager = this._serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRolesAsync(roleManager);

            await SeedUserAsync(userManager, AdminUserName, AdminPassword, AdminRoleName,
                "Admin User", "123 Admin Street", "Admin City", "0000");

            await SeedUserAsync(userManager, ManagerUserName, ManagerPassword, ManagerRoleName,
                "Manager User", "456 Manager Blvd", "Manager Town", "1111");

            await SeedUserAsync(userManager, DefaultUserName, DefaultUserPassword, UserRoleName,
                "Default User", "789 User Road", "User City", "2222");

            await AssignUserRoleToUsersWithoutRolesAsync(userManager);
        }

        private async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
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

        private async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string email, string password,
            string role, string fullName, string address, string city, string postalCode)
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

                if (!createResult.Succeeded)
                {
                    string errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user {email}: {errors}");
                }

                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                ICollection<string> roles = await userManager.GetRolesAsync(existingUser);

                if (!roles.Contains(role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                }
            }
        }

        private async Task AssignUserRoleToUsersWithoutRolesAsync(UserManager<ApplicationUser> userManager)
        {
            ICollection<ApplicationUser> users = await userManager.Users.ToListAsync();

            foreach (ApplicationUser user in users)
            {
                ICollection<string> roles = await userManager.GetRolesAsync(user);

                if (roles == null || roles.Count == 0)
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, UserRoleName);

                    if (!result.Succeeded)
                    {
                        string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to assign default role to user {user.Email}: {errorMessages}");
                    }
                }
            }
        }
    }
}
