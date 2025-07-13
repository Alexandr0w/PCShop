using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Search;

namespace PCShop.Web.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return this.View(new SearchResultsViewModel { Query = "" });
                }

                SearchResultsViewModel results = await this._searchService.SearchAsync(query);

                return this.View(results);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
