using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;

namespace PCShop.Services.Core
{
    public class SearchService : ISearchService
    {
        private readonly PCShopDbContext _dbContext;

        public SearchService(PCShopDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            return await this._dbContext
                .Products
                .Where(p => p.Name.Contains(query))
                .ToArrayAsync(); 
        }
    }
}
