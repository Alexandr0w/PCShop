using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;
using static PCShop.GCommon.ApplicationConstants;

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
            Guid? userGuid = null;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedGuid))
            {
                userGuid = parsedGuid;
            }

            var query = this._dbContext
                .Products
                .Include(p => p.ComputersParts)
                    .ThenInclude(cp => cp.Computer)
                .AsNoTracking()
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
                    Price = p.Price,
                    IsAuthor = userGuid.HasValue && p.ComputersParts.Any(cp => cp.Computer != null && cp.Computer.Id == userGuid.Value)
                })
                .ToArrayAsync();

            return products;
        }

        public async Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId)
        {
            DetailsProductViewModel? detailsProductVM = null;

            Guid? userGuid = null;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedGuid))
            {
                userGuid = parsedGuid;
            }

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
                        Price = product.Price,
                        IsAuthor = userGuid.HasValue && product.ComputersParts.Any(cp => cp.Computer != null && cp.Computer.Id == userGuid.Value)
                    };
                }
            }

            return detailsProductVM;
        }

        public async Task<bool> AddProductAsync(string? userId, AddProductInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            ProductType? productType = await this._dbContext
                .ProductsTypes
                .FirstOrDefaultAsync(pt => pt.Id == Guid.Parse(inputModel.ProductTypeId.ToString()));

            if (user == null || productType == null)
            {
                return false;
            }

            string imageUrl = inputModel.ImageUrl ?? string.Empty; 

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ProductsFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/{ImagesFolder}/{ProductsFolder}/" + uniqueFileName;
            }
            if (user != null && productType != null)
            {
                Product newProduct = new Product
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    ImageUrl = imageUrl,
                    ProductTypeId = productType.Id,
                    ProductType = productType
                };

                await this._dbContext.Products.AddAsync(newProduct);
                await this._dbContext.SaveChangesAsync();

                isAdded = true;
            }

            return isAdded;
        }
    }
}
