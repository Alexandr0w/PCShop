using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;

namespace PCShop.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }

        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return this.View(nameof(Index), null);
            }

            var results = await this._searchService.SearchAsync(query);

            return this.View(nameof(Index), results);
        }
    }
}
