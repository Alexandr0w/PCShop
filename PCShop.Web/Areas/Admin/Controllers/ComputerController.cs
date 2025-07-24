using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.Computer;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.AdminDashboard;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class ComputerController : BaseAdminController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<ComputerController> _logger;

        public ComputerController(IAdminService adminService, ILogger<ComputerController> logger)
        {
            this._adminService = adminService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Deleted(int currentPage = 1)
        {
            try
            {
                DeletedComputersListViewModel model = await this._adminService.GetDeletedComputersAsync(currentPage);
                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(AdminDashboard.DeletedComputersError, ex.Message));
                TempData["ErrorMessage"] = DeletedComputersFailed;

                return this.RedirectToAction(nameof(Deleted), "Computer");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidComputerId;
                    return this.RedirectToAction(nameof(Deleted));
                }

                bool result = await this._adminService.RestoreComputerAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ComputerRestoredSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = ComputerRestoredFailed;
                }

                return this.RedirectToAction(nameof(Deleted));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(AdminDashboard.RestoreComputerError, id, ex.Message));
                TempData["ErrorMessage"] = ComputerRestoredFailed;

                return this.RedirectToAction(nameof(Deleted));
            }
        }

        [HttpPost]
        public async Task<IActionResult> HardDelete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidComputerId;
                    return this.RedirectToAction(nameof(Deleted));
                }

                bool result = await this._adminService.DeleteComputerPermanentlyAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ComputerDeletedPermanentlySuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = ComputerDeletedPermanentlyFailed;
                }

                return this.RedirectToAction(nameof(Deleted));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(AdminDashboard.HardDeleteComputerError, id, ex.Message));
                TempData["ErrorMessage"] = HardDeleteComputerError;

                return this.RedirectToAction(nameof(Deleted));
            }
        }
    }
}
