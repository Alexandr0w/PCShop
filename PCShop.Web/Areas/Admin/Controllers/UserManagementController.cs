using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.UserManagement;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.UserManagement;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class UserManagementController : BaseAdminController
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserManagementService userManagementService, ILogger<UserManagementController> logger)
        {
            this._userManagementService = userManagementService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<UserManagementIndexViewModel> allUsers = await this._userManagementService.GetAllUsersAsync(includeDeleted: true);

                UserManagementPageViewModel viewModel = new UserManagementPageViewModel
                {
                    Users = allUsers
                };

                return this.View(viewModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.IndexError, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    TempData["ErrorMessage"] = InvalidUserId;
                    return this.RedirectToAction(nameof(Index));
                }

                bool userExists = await this._userManagementService.UserExistsByIdAsync(userGuid.ToString());

                if (!userExists)
                {
                    TempData["ErrorMessage"] = UserNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                bool assignResult = await this._userManagementService.AssignUserToRoleAsync(userGuid.ToString(), role);

                if (assignResult)
                {
                    TempData["SuccessMessage"] = string.Format(AssignRoleSuccessfully, role);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(AssignRoleFailed, role);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.AssignRoleError, role, ex.Message, userId));
                TempData["ErrorMessage"] = string.Format(AssignRoleFailed, role);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    TempData["ErrorMessage"] = InvalidUserId;
                    return this.RedirectToAction(nameof(Index));
                }

                bool userExists = await this._userManagementService.UserExistsByIdAsync(userGuid.ToString());

                if (!userExists)
                {
                    TempData["ErrorMessage"] = UserNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                bool removeResult = await this._userManagementService.RemoveUserRoleAsync(userGuid.ToString(), role);

                if (removeResult)
                {
                    TempData["SuccessMessage"] = string.Format(RemoveRoleSuccessfully, role);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(RemoveRoleFailed, role);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.RemoveRoleError, role, ex.Message, userId));
                TempData["ErrorMessage"] = string.Format(RemoveRoleFailed, role);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    TempData["ErrorMessage"] = InvalidUserId;
                    return this.RedirectToAction(nameof(Index));
                }

                bool userExists = await this._userManagementService.UserExistsByIdAsync(userGuid.ToString());

                if (!userExists)
                {
                    TempData["ErrorMessage"] = UserNotFound;
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this._userManagementService.DeleteUserAsync(userGuid.ToString());

                if (deleteResult)
                {
                    TempData["SuccessMessage"] = DeletedUserSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = DeleteUserFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.DeleteUserError, ex.Message, userId));
                TempData["ErrorMessage"] = DeleteUserFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreUser(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    TempData["ErrorMessage"] = InvalidUserId;
                    return this.RedirectToAction(nameof(Index));
                }

                bool success = await this._userManagementService.RestoreUserAsync(userGuid.ToString());

                if (success)
                {
                    TempData["SuccessMessage"] = RestoreUserSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = RestoreUserFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.RestoreUserError, ex.Message, userId));
                TempData["ErrorMessage"] = RestoreUserFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserForever(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    TempData["ErrorMessage"] = InvalidUserId;
                    return this.RedirectToAction(nameof(Index));
                }

                bool success = await this._userManagementService.DeleteUserForeverAsync(userGuid.ToString());

                if (success)
                {
                    TempData["SuccessMessage"] = DeleteUserPermanentlySuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = DeleteUserPermanentlyFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(UserManagement.DeleteUserForeverError, ex.Message, userId));
                TempData["ErrorMessage"] = DeleteUserPermanentlyFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}