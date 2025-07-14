using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ExceptionMessages;

namespace PCShop.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly PCShopDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductService(PCShopDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string? userId, string? productType = null)
        {
            IQueryable<Product> query = this._dbContext
                .Products
                .Include(p => p.ComputersParts)
                    .ThenInclude(cp => cp.Computer)
                .AsNoTracking()
                .Where(p => p.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.ProductType.Name == productType);
            }

            IEnumerable<ProductIndexViewModel> products = await query
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

        public async Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId)
        {
            DetailsProductViewModel? detailsProductVM = null;

            if (productId != null)
            {
                Product? product = await this._dbContext
                    .Products
                    .Include(p => p.ProductType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.ToString() == productId && !p.IsDeleted);

                if (product != null)
                {
                    detailsProductVM = new DetailsProductViewModel
                    {
                        Id = product.Id.ToString(),
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Description = product.Description,
                        ProductType = product.ProductType.Name,
                        Price = product.Price
                    };
                }
            }

            return detailsProductVM;
        }

        public async Task<bool> AddProductAsync(string? userId, ProductFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (!Guid.TryParse(inputModel.ProductTypeId, out Guid productTypeId))
            {
                throw new FormatException(InvalidProductTypeIdFormat);
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            ProductType? productType = await this._dbContext
                .ProductsTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == productTypeId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && productType != null)
            {
                Product newProduct = new Product
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    ImageUrl = imageUrl,
                    ProductTypeId = productType.Id
                };

                await this._dbContext.Products.AddAsync(newProduct);
                await this._dbContext.SaveChangesAsync();

                isAdded = true;
            }

            return isAdded;
        }

        public async Task<ProductFormInputModel?> GetProductForEditingAsync(string productId)
        {
            ProductFormInputModel? editModel = null;

            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException(ProductIdNullOrEmpty);
            }

            Product? productToEdit = await this._dbContext
                .Products
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == Guid.Parse(productId));

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
                    ProductTypes = await this._dbContext
                        .ProductsTypes
                        .Select(pc => new ProductTypeViewModel
                        {
                            Id = pc.Id.ToString(),
                            Name = pc.Name
                        })
                        .ToArrayAsync()
                };
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedProductAsync(string userId, ProductFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isPersisted = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Product? updatedProduct = await this._dbContext
                .Products
                .FindAsync(Guid.Parse(inputModel.Id));

            ProductType? productType = await this._dbContext
                .ProductsTypes
                .FindAsync(Guid.Parse(inputModel.ProductTypeId));

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && updatedProduct != null && productType != null)
            {
                updatedProduct.Name = inputModel.Name;
                updatedProduct.Description = inputModel.Description;
                updatedProduct.Price = inputModel.Price;
                updatedProduct.ImageUrl = imageUrl;
                updatedProduct.ProductTypeId = productType.Id;

                await this._dbContext.SaveChangesAsync();
                isPersisted = true;
            }

            return isPersisted;
        }

        public async Task<DeleteProductViewModel?> GetProductForDeletingAsync(string? userId, string? productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException(ProductIdNullOrEmpty);
            }

            if (!Guid.TryParse(productId, out Guid productGuid))
            {
                throw new FormatException(InvalidProductIdFormat);
            }

            DeleteProductViewModel? deleteModel = null;

            Product? deleteProductModel = await this._dbContext
                .Products
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == productGuid);

            if (deleteProductModel != null)
            {
                deleteModel = new DeleteProductViewModel
                {
                    Id = deleteProductModel.Id.ToString(),
                    Name = deleteProductModel.Name,
                    ImageUrl = deleteProductModel.ImageUrl
                };
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteProductAsync(string userId, DeleteProductViewModel inputModel)
        {
            bool isSuccessDeleted = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid productId))
            {
                throw new FormatException(InvalidProductIdFormat);
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Product? product = await this._dbContext
                .Products
                .FindAsync(productId);

            if (user != null && product != null)
            {
                product.IsDeleted = true;
                isSuccessDeleted = true;

                await this._dbContext.SaveChangesAsync();
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
                    throw new InvalidOperationException(InvalidFileType);
                }

                if (!imageFile.ContentType.StartsWith("image/"))
                {
                    throw new InvalidOperationException(InvalidContentType);
                }

                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ProductsFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/{ImagesFolder}/{ProductsFolder}/" + uniqueFileName;
            }

            return imageUrl;
        }
    }
}