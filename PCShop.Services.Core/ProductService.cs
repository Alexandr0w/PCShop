using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly PCShopDbContext _dbContext;

        public ProductService(PCShopDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string? userId)
        {
            Guid? userGuid = null;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedGuid))
            {
                userGuid = parsedGuid;
            }

            IEnumerable<ProductIndexViewModel> products = await this._dbContext
                .Products
                    .Include(p => p.ComputersParts)
                        .ThenInclude(cp => cp.Computer)
                    .Include(p => p.OrdersItems)
                    .AsNoTracking()
                    .Select(p => new ProductIndexViewModel
                    {
                        Id = p.Id.GetHashCode(),
                        Title = p.Name,
                        ImageUrl = p.ImageUrl,
                        Category = p.ProductType.Name,
                        IsAuthor = userGuid.HasValue && p.ComputersParts.Any(cp => cp.Computer != null && cp.Computer.Id == userGuid.Value), 
                        IsSaved = userGuid.HasValue && p.OrdersItems.Any(oi => oi.Order.ApplicationUserId == userGuid.Value),
                        SavedCount = p.OrdersItems.Count()
                    })
                    .ToArrayAsync();

            return products;
        }
    }
}
