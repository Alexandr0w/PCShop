using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Search;
using static PCShop.Data.Common.EntityConstants.Search;

namespace PCShop.Services.Core
{
    public class SearchService : ISearchService
    {
        private readonly PCShopDbContext _dbContext;

        public SearchService(PCShopDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<SearchResultsViewModel> SearchAsync(string query, int currentPage = SearchCurrentPage, int itemsPerPage = SearchProductsPerPage)
        {
            IQueryable<SearchResultItemViewModel> productQuery = this._dbContext
                .Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                .Select(p => new SearchResultItemViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Type = "Product"
                });

            IQueryable<SearchResultItemViewModel> computerQuery = this._dbContext
                .Computers
                .Where(c => c.Name.ToLower().Contains(query.ToLower()))
                .Select(c => new SearchResultItemViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Price = c.Price,
                    Type = "Computer"
                });

            IQueryable<SearchResultItemViewModel> combinedQuery = productQuery
                .Concat(computerQuery)
                .OrderBy(r => r.Price);

            int totalResults = await combinedQuery.CountAsync();

            ICollection<SearchResultItemViewModel> pagedResults = await combinedQuery
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            return new SearchResultsViewModel
            {
                Query = query,
                Results = pagedResults,
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalResults = totalResults
            };
        }
    }
}
