using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly PCShopDbContext _dbContext;

        public ProductTypeService(PCShopDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductTypeViewModel>> GetProductTypeMenuAsync()
        {
            IEnumerable<ProductTypeViewModel> productTypes = await this._dbContext
                .ProductsTypes
                .AsNoTracking()
                .Select(pt => new ProductTypeViewModel
                {
                    Id = pt.Id.ToString(), 
                    Name = pt.Name
                })
                .ToArrayAsync();

            return productTypes;
        }
    }
}
