using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Admin.ProductManagement;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.ProductMessages;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService _productManagementService;
        private readonly IProductTypeService _productTypeService;
        private readonly ILogger<ProductManagementController> _logger;

        public ProductManagementController(
            IProductManagementService productManagementService,
            IProductTypeService productTypeService,
            ILogger<ProductManagementController> logger)
        {
            this._productManagementService = productManagementService;
            this._productTypeService = productTypeService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int currentPage = 1)
        {
            int productsPerPage = 10;

            IEnumerable<ProductManagementIndexViewModel> allProducts = await this._productManagementService
                .GetAllProductsAsync(includeDeleted: true);

            ICollection<ProductManagementIndexViewModel> pagedProducts = allProducts
                .Skip((currentPage - 1) * productsPerPage)
                .Take(productsPerPage)
                .ToList();

            ProductManagementPageViewModel model = new ProductManagementPageViewModel
            {
                Products = pagedProducts,
                TotalProducts = allProducts.Count(),
                ProductsPerPage = productsPerPage,
                CurrentPage = currentPage
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                ProductManagementFormInputModel model = new ProductManagementFormInputModel
                {
                    ProductTypes = await this._productTypeService.GetProductTypeMenuAsync()
                };

                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.IndexError, ex.Message));
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductManagementFormInputModel inputModel)
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
                    inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                bool result = await this._productManagementService.AddProductAsync(userId, inputModel, inputModel.ImageFile);

                if (!result)
                {
                    inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                    ModelState.AddModelError(string.Empty, $"Error adding product {inputModel.Name}.");
                    
                    TempData["ErrorMessage"] = AddFailed;
                    return this.View(inputModel);
                }
                else
                {
                    TempData["SuccessMessage"] = AddedSuccessfully;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (InvalidOperationException ex)
            {
                inputModel.ProductTypes = await _productTypeService.GetProductTypeMenuAsync();
                ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);

                TempData["ErrorMessage"] = AddFailed;
                return this.View(inputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.AddError, ex.Message));
                TempData["ErrorMessage"] = AddFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.RedirectToAction(nameof(Manage));
                }

                ProductManagementFormInputModel? editProductInputModel = await this._productManagementService
                    .GetProductEditFormModelAsync(id);

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
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductManagementFormInputModel inputModel)
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
                        string uploadedUrl = await this._productManagementService.UploadImageAsync(inputModel, inputModel.ImageFile);
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

                bool editResult = await this._productManagementService.EditProductAsync(userId, inputModel, null);

                if (!editResult)
                {
                    inputModel.ProductTypes = await this._productTypeService.GetProductTypeMenuAsync();

                    ModelState.AddModelError(string.Empty, string.Format(Product.EditError, inputModel.Name));
                    this._logger.LogError(string.Format(Product.EditError, inputModel.Name));

                    TempData["ErrorMessage"] = EditeFailed;
                    return this.View(inputModel);
                }
                else
                {
                    TempData["SuccessMessage"] = EditedSuccessfully;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.EditError, ex.Message));
                TempData["ErrorMessage"] = EditeFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool result = await this._productManagementService.SoftDeleteProductAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = DeletedSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = DeleteFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.SoftDeleteError, ex.Message));
                TempData["ErrorMessage"] = DeleteFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidProductId;
                    return this.RedirectToAction(nameof(Manage));
                }

                bool result = await this._productManagementService.RestoreProductAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ProductRestoredSuccessfully; 
                }
                else
                {
                    TempData["ErrorMessage"] = ProductRestoredFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.RestoreProductError, id, ex.Message));
                TempData["ErrorMessage"] = ProductRestoredFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForever(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidProductId;
                    return this.RedirectToAction(nameof(Manage));
                }

                bool result = await this._productManagementService.DeleteProductPermanentlyAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ProductDeletedPermanentlySuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = ProductDeletedPermanentlyFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Product.HardDeleteProductError, id, ex.Message));
                TempData["ErrorMessage"] = ProductDeletedPermanentlyFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }
    }
}