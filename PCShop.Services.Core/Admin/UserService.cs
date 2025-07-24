using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Admin.UserManagement;
using System.Data;

namespace PCShop.Services.Core.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly INotificationService _notificationService;

        public UserService(UserManager<
            ApplicationUser> userManager, 
            RoleManager<IdentityRole<Guid>> roleManager,
            INotificationService notificationService)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._notificationService = notificationService;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(bool includeDeleted = true)
        {
            IQueryable<ApplicationUser> query = this._userManager
                .Users
                .AsQueryable()
                .IgnoreQueryFilters();

            if (!includeDeleted)
            {
                query = query.Where(u => !u.IsDeleted);
            }

            ICollection<ApplicationUser> allUsers = await query
                .ToListAsync();

            List<UserManagementIndexViewModel> allUsersViewModel = new List<UserManagementIndexViewModel>();

            foreach (ApplicationUser user in allUsers)
            {
                IEnumerable<string> roles = await this._userManager.GetRolesAsync(user);

                allUsersViewModel.Add(new UserManagementIndexViewModel
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = roles,
                    IsDeleted = user.IsDeleted
                });
            }

            return allUsersViewModel;
        }

        public async Task<bool> UserExistsByIdAsync(string userId)
        {
            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId.ToString());

            return user != null;
        }

        public async Task<bool> AssignUserToRoleAsync(string userId, string roleName)
        {
            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId.ToString());

            bool roleExists = await this._roleManager.RoleExistsAsync(roleName);

            if (user == null || !roleExists)
            {
                return false;
            }

            bool alreadyInRole = await this._userManager.IsInRoleAsync(user, roleName);

            if (!alreadyInRole)
            {
                IdentityResult? result = await this._userManager
                    .AddToRoleAsync(user, roleName);

                string roleMessage = $"Your role has been updated to '{roleName}'.";
                await this._notificationService.CreateAsync(user.Id.ToString(), roleMessage);

                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> RemoveUserRoleAsync(string userId, string roleName)
        {
            ApplicationUser? user = await this._userManager
                            .FindByIdAsync(userId.ToString());

            bool roleExists = await this._roleManager.RoleExistsAsync(roleName);

            if (user == null || !roleExists)
            {
                return false;
            }

            bool alreadyInRole = await this._userManager.IsInRoleAsync(user, roleName);

            if (alreadyInRole)
            {
                IdentityResult? result = await this._userManager
                    .RemoveFromRoleAsync(user, roleName);

                string roleMessage = $"Your role '{roleName}' has been removed.";
                await this._notificationService.CreateAsync(user.Id.ToString(), roleMessage);

                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await this._userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;

            IdentityResult result = await this._userManager.UpdateAsync(user);

            string deleteMessage = "Your profile has been soft-deleted.";
            await this._notificationService.CreateAsync(user.Id.ToString(), deleteMessage);

            return result.Succeeded;
        }

        public async Task<bool> RestoreUserAsync(string userId)
        {
            ApplicationUser? user = await this._userManager
                .Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null || !user.IsDeleted)
            {
                return false;
            }

            user.IsDeleted = false;
            IdentityResult result = await this._userManager.UpdateAsync(user);

            string restoreMessage = "Your profile has been restored.";
            await this._notificationService.CreateAsync(user.Id.ToString(), restoreMessage);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserForeverAsync(string userId)
        {
            ApplicationUser? user = await this._userManager
                .Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null || !user.IsDeleted)
            {
                return false;
            }

            IdentityResult result = await this._userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}