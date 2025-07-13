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
        private readonly IProductTypeService _productTypeService;

        public ProductController(IProductService productService, IProductTypeService productTypeService)
        {
            this._productService = productService;
            this._productTypeService = productTypeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? productType)
        {
            try
            {
                string? userId = this.GetUserId();

                IEnumerable<ProductIndexViewModel> allProducts = await this._productService.GetAllProductsAsync(userId, productType);

                return this.View(allProducts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(productDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddProductInputModel addProductInputModel = new AddProductInputModel
                {
                    ProductTypes = await this._productTypeService.GetProductTypeMenuAsync()
                };

                return this.View(addProductInputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddProductInputModel inputModel, IFormFile? imageFile)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();
                    return this.View(inputModel);
                }

                bool addResult = await this._productService.AddProductAsync(this.GetUserId(), inputModel, imageFile);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, AddProductErrorMessage);
                    inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
