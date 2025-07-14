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
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService,
                                 IProductTypeService productTypeService,
                                 ILogger<ProductController> logger)
        {
            this._productService = productService;
            this._productTypeService = productTypeService;
            this._logger = logger;
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
                this._logger.LogError(e.Message);
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
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            try
            {
                ProductFormInputModel addProductInputModel = new ProductFormInputModel
                {
                    ProductTypes = await this._productTypeService.GetProductTypeMenuAsync()
                };

                return this.View(addProductInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ProductFormInputModel inputModel, IFormFile? imageFile)
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
                    this._logger.LogError(AddProductErrorMessage);

                    inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.RedirectToAction(nameof(Index));
                }

                ProductFormInputModel? editProductInputModel = await this._productService.GetProductForEditingAsync(id);

                if (editProductInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                editProductInputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                return this.View(editProductInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ProductFormInputModel inputModel, IFormFile? imageFile)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();
                    return this.View(inputModel);
                }

                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                bool editResult = await this._productService.PersistUpdatedProductAsync(userId, inputModel, imageFile);

                if (editResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, EditProductErrorMessage);
                    this._logger.LogError(EditProductErrorMessage);

                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                string? userId = this.GetUserId();

                DeleteProductViewModel? deleteProductInputModel = await this._productService.GetProductForDeletingAsync(userId, id);

                if (deleteProductInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(deleteProductInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteProductViewModel inputModel)
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, NotModifyMessage);
                    this._logger.LogError(NotModifyMessage);

                    return this.View(inputModel);
                }

                bool deleteResult = await this._productService.SoftDeleteProductAsync(userId, inputModel);

                if (deleteResult == false)
                {
                    ModelState.AddModelError(string.Empty, DeleteProductErrorMessage);
                    this._logger.LogError(DeleteProductErrorMessage);

                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}