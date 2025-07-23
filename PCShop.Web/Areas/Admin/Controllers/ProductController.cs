using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IAdminService _adminService;

        public ProductController(IAdminService adminService)
        {
            this._adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Deleted(int currentPage = 1)
        {
            var model = await this._adminService.GetDeletedProductsAsync(currentPage);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            bool result = await this._adminService.RestoreProductAsync(id);

            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "Missing product ID.";
                return RedirectToAction(nameof(Deleted));
            }

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to restore product.";
            }
            else
            {
                TempData["SuccessMessage"] = "Product restored successfully.";
            }

            return RedirectToAction(nameof(Deleted));
        }

        [HttpPost]
        public async Task<IActionResult> HardDelete(string id)
        {
            bool result = await this._adminService.DeleteProductPermanentlyAsync(id);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to permanently delete product.";
            }
            else
            {
                TempData["SuccessMessage"] = "Product permanently deleted.";
            }

            return this.RedirectToAction(nameof(Deleted));
        }
    }
}