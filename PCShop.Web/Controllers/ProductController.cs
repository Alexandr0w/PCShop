using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;
using static PCShop.GCommon.ErrorMessages;

namespace PCShop.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger)
        {
            this._productService = productService;
            this._logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] ProductListViewModel queryModel)
        {
            try
            {
                string? userId = this.GetUserId();

                await this._productService.GetAllProductsQueryAsync(queryModel);
                return this.View(queryModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.IndexError, ex.Message));
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                string? userId = this.GetUserId();

                DetailsProductViewModel? productDetails = await this._productService.GetProductDetailsAsync(userId, id);

                if (productDetails == null)
                {
                    return this.NotFound();
                }

                return this.View(productDetails);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.DetailsError, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}