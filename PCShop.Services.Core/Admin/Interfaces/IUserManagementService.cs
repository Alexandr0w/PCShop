using PCShop.Web.ViewModels.Admin.UserManagement;

namespace PCShop.Services.Core.Admin.Interfaces
{
    public interface IUserManagementService
    {
        Task GetAllUsersAsync(UserManagementPageViewModel model);

        Task<bool> UserExistsByIdAsync(string userId);

        Task<bool> AssignUserToRoleAsync(string userId, string roleName);

        Task<bool> RemoveUserRoleAsync(string userId, string roleName);

        Task<bool> DeleteUserAsync(string userId);

        Task<bool> RestoreUserAsync(string userId);
        Task<bool> DeleteUserForeverAsync(string userId);
    }
}
