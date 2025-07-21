using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class ProductController : Controller
    {
        private readonly IAdminService _productService;

        public ProductController(IAdminService productService)
        {
            this._productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Deleted(int currentPage = 1)
        {
            var model = await this._productService.GetDeletedProductsAsync(currentPage);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            bool result = await this._productService.RestoreProductAsync(id);

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
            bool result = await this._productService.DeleteProductPermanentlyAsync(id);

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