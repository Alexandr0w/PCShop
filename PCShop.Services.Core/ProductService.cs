using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using static PCShop.Services.Common.ServiceConstants;
using PCShop.Web.ViewModels.Product;
using static PCShop.GCommon.ExceptionMessages;

namespace PCShop.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductService(
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            UserManager<ApplicationUser> userManager)
        {
            this._productRepository = productRepository;
            this._productTypeRepository = productTypeRepository;
            this._userManager = userManager;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string? userId, string? productType = null)
        {
            IQueryable<Product> productsQuery = this._productRepository
                .GetAllAttached()
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(productType))
            {
                productsQuery = productsQuery.Where(p => p.ProductType.Name == productType);
            }

            IEnumerable<ProductIndexViewModel> products = await productsQuery
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    ProductType = p.ProductType.Name,
                    Price = p.Price
                })
                .ToArrayAsync();

            return products;
        }

        public async Task PopulateProductQueryModelAsync(ProductListViewModel model, string? userId)
        {
            IQueryable<Product> productsQuery = this._productRepository
                .GetAllAttached()
                .Where(p => !p.IsDeleted)
                .Include(p => p.ProductType)
                .AsNoTracking();

            model.AllProductTypes = await productsQuery
                .Select(p => p.ProductType.Name)
                .Distinct()
                .OrderBy(t => t)
                .ToArrayAsync();

            if (!string.IsNullOrWhiteSpace(model.ProductType))
            {
                productsQuery = productsQuery.Where(p => p.ProductType.Name == model.ProductType);
            }

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                string search = model.SearchTerm.ToLower();
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(search));
            }

            model.TotalProducts = await productsQuery.CountAsync();

            productsQuery = model.SortOption switch
            {
                "name_asc" => productsQuery.OrderBy(p => p.Name),
                "name_desc" => productsQuery.OrderByDescending(p => p.Name),
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                _ => productsQuery.OrderByDescending(p => p.Id)
            };

            model.Products = await productsQuery
                .Skip((model.CurrentPage - 1) * model.ProductsPerPage)
                .Take(model.ProductsPerPage)
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    ProductType = p.ProductType.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToArrayAsync();
        }

        public async Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId)
        {
            DetailsProductViewModel? detailsProductVM = null;

            bool isIdValidGuid = Guid.TryParse(productId, out Guid productIdGuid);

            if (isIdValidGuid)
            {
                detailsProductVM = await this._productRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(p => p.Id == productIdGuid)
                    .Select(p => new DetailsProductViewModel()
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Description = p.Description,
                        ProductType = p.ProductType.Name,
                        ImageUrl = p.ImageUrl,
                        Price = p.Price
                    })
                    .SingleOrDefaultAsync();
            }

            return detailsProductVM;
        }

        public async Task<bool> AddProductAsync(string? userId, ProductFormInputModel inputModel, IFormFile? imageFile)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.ProductTypeId, out Guid productTypeId))
            {
                throw new FormatException(InvalidProductTypeIdFormatMessage);
            }

            bool isAdded = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            ProductType? productType = await this._productTypeRepository
                .GetByIdAsync(productTypeId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && productType != null)
            {
                Product product = new Product
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    ImageUrl = imageUrl,
                    ProductTypeId = productType.Id
                };

                await this._productRepository.AddAsync(product);

                isAdded = true;
            }
            return isAdded;
        }

        public async Task<ProductFormInputModel?> GetProductForEditingAsync(string? productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException(ProductIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(productId, out Guid productGuid))
            {
                throw new FormatException(ProductIdNullOrEmptyMessage);
            }

            ProductFormInputModel? editModel = null;
            
            Product? productToEdit = await this._productRepository
                .GetByIdAsync(productGuid);

            IEnumerable<ProductType> productTypes = await this._productTypeRepository
                .GetAllAsync();

            if (productToEdit != null)
            {
                editModel = new ProductFormInputModel
                {
                    Id = productToEdit.Id.ToString(),
                    Name = productToEdit.Name,
                    Description = productToEdit.Description,
                    Price = productToEdit.Price,
                    ImageUrl = productToEdit.ImageUrl,
                    ProductTypeId = productToEdit.ProductTypeId.ToString(),
                    ProductTypes = productTypes
                    .Select(pt => new ProductTypeViewModel
                    {
                        Id = pt.Id.ToString(),
                        Name = pt.Name
                    })
                };
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedProductAsync(string userId, ProductFormInputModel inputModel, IFormFile? imageFile)
        {
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

            bool isPersisted = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Product? updatedProduct = await this._productRepository
                .GetByIdAsync(productId);

            ProductType? productType = await this._productTypeRepository
                .GetByIdAsync(productTypeId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && updatedProduct != null && productType != null)
            {
                updatedProduct.Name = inputModel.Name;
                updatedProduct.Description = inputModel.Description;
                updatedProduct.Price = inputModel.Price;
                updatedProduct.ImageUrl = imageUrl;
                updatedProduct.ProductTypeId = productType.Id;

                await this._productRepository.UpdateAsync(updatedProduct);
                
                isPersisted = true;
            }

            return isPersisted;
        }

        public async Task<DeleteProductViewModel?> GetProductForDeletingAsync(string? userId, string? productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(productId, out Guid productGuid))
            {
                throw new FormatException(InvalidProductIdFormatMessage);
            }

            DeleteProductViewModel? deleteModel = null;

            Product? productToDelete = await this._productRepository
                .GetByIdAsync(productGuid);

            if (productToDelete != null)
            {
                deleteModel = new DeleteProductViewModel
                {
                    Id = productToDelete.Id.ToString(),
                    Name = productToDelete.Name,
                    ImageUrl = productToDelete.ImageUrl
                };
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteProductAsync(string userId, DeleteProductViewModel inputModel)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid productId))
            {
                throw new FormatException(InvalidProductIdFormatMessage);
            }

            bool isSuccessDeleted = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Product? product = await this._productRepository
                .GetByIdAsync(productId);

            if (user != null && product != null)
            {
                await this._productRepository.DeleteAsync(product);

                isSuccessDeleted = true;
            }

            return isSuccessDeleted;
        }

        public async Task<string> UploadImageAsync(ProductFormInputModel inputModel, IFormFile? imageFile)
        {
            string imageUrl = inputModel.ImageUrl ?? string.Empty;

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

                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ProductsFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fileStream);

                imageUrl = $"/{ImagesFolder}/{ProductsFolder}/" + uniqueFileName;
            }

            return imageUrl;
        }
    }
}
