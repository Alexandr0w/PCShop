using PCShop.Web.ViewModels.Admin.UserManagement;

namespace PCShop.Services.Core.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetAllUsersAsync(bool includeDeleted = true);

        Task<IEnumerable<string>> GetAvailableRolesAsync();

        Task<bool> UserExistsByIdAsync(string userId);

        Task<bool> AssignUserToRoleAsync(string userId, string roleName);

        Task<bool> RemoveUserRoleAsync(string userId, string roleName);

        Task<bool> DeleteUserAsync(string userId);

        Task<bool> RestoreUserAsync(string userId);
        Task<bool> DeleteUserForeverAsync(string userId);
    }
}
