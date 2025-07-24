using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.UserManagement;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
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

            ICollection<ApplicationUser> allUsers = await query.ToListAsync();

            ICollection<UserManagementIndexViewModel> allUsersViewModel = new List<UserManagementIndexViewModel>();

            foreach (ApplicationUser user in allUsers)
            {
                ICollection<string> roles = await this._userManager.GetRolesAsync(user);

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

        public async Task<IEnumerable<string>> GetAvailableRolesAsync()
        {
            string[] rolesName = { AdminRoleName, ManagerRoleName, UserRoleName };

            List<string> roles = await this._roleManager
                .Roles
                .Where(r => rolesName.Contains(r.Name))
                .Select(r => r.Name!)
                .ToListAsync();

            return roles;
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