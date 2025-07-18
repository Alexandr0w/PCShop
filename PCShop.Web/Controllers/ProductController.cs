using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.GCommon.MessageConstants.ProductMessages;

namespace PCShop.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductTypeService _productTypeService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            IProductTypeService productTypeService,
            ILogger<ProductController> logger)
        {
            this._productService = productService;
            this._productTypeService = productTypeService;
            this._logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] ProductListViewModel queryModel)
        {
            try
            {
                string? userId = this.GetUserId();

                await this._productService.PopulateProductQueryModelAsync(queryModel, userId);
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
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.AddPage, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ProductFormInputModel inputModel)
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (!this.ModelState.IsValid)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                bool addResult = await this._productService.AddProductAsync(userId, inputModel, inputModel.ImageFile);

                if (addResult == false)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(string.Empty, string.Format(Product.AddError, inputModel.Name));
                    this._logger.LogError(string.Format(Product.AddError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = AddedSuccessfully;

                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();

                ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                this._logger.LogError(ex, InvalidFileTypeMessage);

                return this.View(inputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.AddError, ex.Message));
                TempData["ErrorMessage"] = AddFailed;

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
                    return this.NotFound();
                }

                editProductInputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();
                return this.View(editProductInputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.EditPage, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ProductFormInputModel inputModel)
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
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                try
                {
                    if (inputModel.ImageFile != null && inputModel.ImageFile.Length > 0)
                    {
                        string uploadedUrl = await this._productService.UploadImageAsync(inputModel, inputModel.ImageFile);
                        inputModel.ImageUrl = uploadedUrl;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                    this._logger.LogError(ex, ex.Message);

                    return this.View(inputModel);
                }

                bool editResult = await this._productService.PersistUpdatedProductAsync(userId, inputModel, null);

                if (editResult == false)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(string.Empty, string.Format(Product.EditError, inputModel.Name));
                    this._logger.LogError(string.Format(Product.EditError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = UpdatedSuccessfully;

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.EditError, ex.Message));
                TempData["ErrorMessage"] = UpdateFailed;

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
                    return this.NotFound();
                }

                return this.View(deleteProductInputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.DeletePage, ex.Message));
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
                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                bool deleteResult = await this._productService.SoftDeleteProductAsync(userId, inputModel);

                if (deleteResult == false)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Product.DeleteError, inputModel.Name));
                    this._logger.LogError(string.Format(Product.DeleteError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = DeletedSuccessfully;

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.DeleteError, ex.Message));
                TempData["ErrorMessage"] = DeleteFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}