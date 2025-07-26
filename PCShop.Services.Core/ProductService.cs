using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;
using System.Globalization;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<ProductListViewModel> GetAllProductsQueryAsync(ProductListViewModel model)
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
                "oldest" => productsQuery.OrderBy(p => p.CreatedOn),
                "default" or null or "" => productsQuery.OrderByDescending(p => p.CreatedOn),
                _ => productsQuery.OrderByDescending(p => p.CreatedOn)
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

            return model;
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
                        Price = p.Price,
                        CreatedOn = p.CreatedOn.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture)
                    })
                    .SingleOrDefaultAsync();
            }

            return detailsProductVM;
        }
    }
}
