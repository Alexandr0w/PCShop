using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Admin;

namespace PCShop.Services.Core
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _productRepository;

        public AdminService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<DeletedProductsListViewModel> GetDeletedProductsAsync(int currentPage = 1, int pageSize = 12)
        {
            IQueryable<Product> query = this._productRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted)
                .Include(p => p.ProductType)
                .IgnoreQueryFilters();

            int totalCount = await query.CountAsync();

            IEnumerable<DeletedProductViewModel> products = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new DeletedProductViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    ProductType = p.ProductType.Name,
                    DeletedOn = p.DeletedOn
                })
                .ToListAsync();

            return new DeletedProductsListViewModel
            {
                CurrentPage = currentPage,
                ProductsPerPage = pageSize,
                TotalProducts = totalCount,
                Products = products
            };
        }

        public async Task<bool> RestoreProductAsync(string productId)
        {
            Product? product = await this._productRepository
                .GetByIdAsync(Guid.Parse(productId));

            if (product == null || product.IsDeleted == false)
            {
                return false;
            }

            product.IsDeleted = false;

            return await this._productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductPermanentlyAsync(string productId)
        {
            Product? product = await this._productRepository
                .GetByIdAsync(Guid.Parse(productId));

            if (product == null || !product.IsDeleted)
            {
                return false;
            }

            return await this._productRepository.HardDeleteAsync(product);
        }
    }
}
