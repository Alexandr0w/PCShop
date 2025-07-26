using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.ProductManagement;
using System.Globalization;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Admin
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductManagementService(
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._productRepository = productRepository;
            this._productTypeRepository = productTypeRepository;
            this._userManager = userManager;
        }

        public async Task<ProductManagementPageViewModel> GetAllProductsAsync(ProductManagementPageViewModel model)
        {
            IQueryable<Product> productQuery = this._productRepository
                .GetAllAttached()
                .Include(p => p.ProductType)
                .AsNoTracking()
                .IgnoreQueryFilters();

            model.TotalProducts = await productQuery
                .CountAsync();

            model.Products = await productQuery
                .OrderByDescending(p => p.CreatedOn)
                .Skip((model.CurrentPage - 1) * model.ProductsPerPage)
                .Take(model.ProductsPerPage)
                .Select(p => new ProductManagementIndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    CreatedOn = p.CreatedOn,
                    ProductType = p.ProductType.Name,
                    IsDeleted = p.IsDeleted,
                    DeletedOn = p.DeletedOn
                })
                .ToListAsync();

            return model;
        }

        public async Task<bool> AddProductAsync(string? userId, ProductManagementFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.ProductTypeId, out Guid productTypeId))
            {
                throw new FormatException(InvalidProductTypeIdFormatMessage);
            }

            bool isCreatedOnValid = DateTime.TryParseExact(inputModel.CreatedOn, DateAndTimeInputFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOn);

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            ProductType? productType = await this._productTypeRepository
                .GetByIdAsync(productTypeId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && productType != null && isCreatedOnValid)
            {
                Product product = new Product
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    CreatedOn = createdOn,
                    ImageUrl = imageUrl,
                    ProductTypeId = productType.Id
                };

                await this._productRepository.AddAsync(product);
                
                isAdded = true;
            }

            return isAdded;
        }

        public async Task<ProductManagementFormInputModel?> GetProductEditFormModelAsync(string? productId)
        {
            ProductManagementFormInputModel? editModel = null;

            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException(ProductIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(productId, out Guid productGuid))
            {
                throw new FormatException(ProductIdNullOrEmptyMessage);
            }

            Product? productToEdit = await this._productRepository
                .GetByIdAsync(productGuid);

            IEnumerable<ProductType> productTypes = await this._productTypeRepository
                .GetAllAsync();

            if (productToEdit != null)
            {
                editModel = new ProductManagementFormInputModel
                {
                    Id = productToEdit.Id.ToString(),
                    Name = productToEdit.Name,
                    Description = productToEdit.Description,
                    Price = productToEdit.Price,
                    CreatedOn = productToEdit.CreatedOn.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture),
                    ImageUrl = productToEdit.ImageUrl,
                    ProductTypeId = productToEdit.ProductTypeId.ToString(),
                    ProductTypes = await this._productTypeRepository.GetAllProductTypeViewModelsAsync()
                };
            }

            return editModel;
        }

        public async Task<bool> EditProductAsync(string userId, ProductManagementFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isEdited = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid productId))
            {
                throw new FormatException(InvalidProductIdFormatMessage);
            }

            if (!Guid.TryParse(inputModel.ProductTypeId, out Guid productTypeId))
            {
                throw new FormatException(InvalidProductTypeIdFormatMessage);
            }

            bool isCreatedOnValid = DateTime
                .TryParseExact(inputModel.CreatedOn, DateAndTimeDisplayFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOn);

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Product? updatedProduct = await this._productRepository
                .GetByIdAsync(productId);

            ProductType? productType = await this._productTypeRepository
                .GetByIdAsync(productTypeId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && updatedProduct != null && productType != null && isCreatedOnValid)
            {
                updatedProduct.Name = inputModel.Name;
                updatedProduct.Description = inputModel.Description;
                updatedProduct.Price = inputModel.Price;
                updatedProduct.CreatedOn = createdOn;
                updatedProduct.ImageUrl = imageUrl;
                updatedProduct.ProductTypeId = productType.Id;

                await this._productRepository.UpdateAsync(updatedProduct);
                isEdited = true;
            }

            return isEdited;
        }

        public async Task<bool> SoftDeleteProductAsync(string? productId)
        {
            bool isAlreadyDeleted = false;

            if (!string.IsNullOrWhiteSpace(productId))
            {
                Product? product = await this._productRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(p => p.Id.ToString().ToLower() == productId.ToString().ToLower());

                if (product != null)
                {
                    product.IsDeleted = true;
                    product.DeletedOn = DateTime.UtcNow;

                    await this._productRepository.UpdateAsync(product);
                    isAlreadyDeleted = true;
                }
            }

            return isAlreadyDeleted;
        }

        public async Task<bool> RestoreProductAsync(string productId)
        {
            bool isRestored = false;

            if (!string.IsNullOrWhiteSpace(productId))
            {
                Product? product = await this._productRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == productId.ToLower());

                if (product != null && product.IsDeleted)
                {
                    product.IsDeleted = false;
                    product.DeletedOn = null;

                    await this._productRepository.UpdateAsync(product);
                    isRestored = true;
                }
            }

            return isRestored;
        }

        public async Task<bool> DeleteProductPermanentlyAsync(string productId)
        {
            bool isPermanent = false;

            if (!string.IsNullOrWhiteSpace(productId))
            {
                Product? product = await this._productRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == productId.ToLower());

                if (product != null && product.IsDeleted)
                {
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), RootFolder, ImagesFolder, ProductsFolder,
                            Path.GetFileName(product.ImageUrl));

                        if (File.Exists(imagePath))
                        {
                            try
                            {
                                File.Delete(imagePath);
                            }
                            catch (Exception ex)
                            {
                                string errorMessage = $"Failed to delete product image '{imagePath}'.";
                                throw new IOException(errorMessage, ex);
                            }
                        }
                    }

                    await this._productRepository.HardDeleteAsync(product);
                    isPermanent = true;
                }
            }

            return isPermanent;
        }

        public async Task<string> UploadImageAsync(ProductManagementFormInputModel inputModel, IFormFile? imageFile)
        {
            string existingImageUrl = inputModel.ImageUrl ?? string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException(InvalidFileTypeMessage);
                }

                if (!imageFile.ContentType.StartsWith("image/"))
                {
                    throw new InvalidOperationException(InvalidContentTypeMessage);
                }

                // Delete old image if it exists 
                if (!string.IsNullOrWhiteSpace(existingImageUrl) && existingImageUrl.StartsWith($"/{ImagesFolder}/{ProductsFolder}/"))
                {
                    string oldImagePath = Path.Combine(RootFolder, existingImageUrl.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ProductsFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fileStream);

                existingImageUrl = $"/{ImagesFolder}/{ProductsFolder}/" + uniqueFileName;
            }

            if (string.IsNullOrWhiteSpace(existingImageUrl))
            {
                throw new InvalidOperationException(ImageNotFoundMessage);
            }

            return existingImageUrl;
        }
    }
}

