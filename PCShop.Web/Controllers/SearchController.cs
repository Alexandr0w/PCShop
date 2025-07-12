using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Search;

namespace PCShop.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return this.View(new SearchResultsViewModel { Query = "" });
            }

            var results = await this._searchService.SearchAsync(query);

            return this.View(results);
        }
    }
}
