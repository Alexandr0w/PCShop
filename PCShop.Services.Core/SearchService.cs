using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Search;

namespace PCShop.Services.Core
{
    public class SearchService : ISearchService
    {
        private readonly PCShopDbContext _dbContext;

        public SearchService(PCShopDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<SearchResultsViewModel> SearchAsync(string query)
        {
            IEnumerable<ProductSearchResultViewModel> products = await this._dbContext
                .Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                .Select(p => new ProductSearchResultViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price
                })
                .ToListAsync();

            IEnumerable<ComputerSearchResultViewModel> computers = await this._dbContext
                .Computers
                .Where(c => c.Name.ToLower().Contains(query.ToLower()))
                .Select(c => new ComputerSearchResultViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Price = c.Price
                })
                .ToListAsync();

            return new SearchResultsViewModel
            {
                Query = query,
                Products = products,
                Computers = computers
            };
        }
    }
}
