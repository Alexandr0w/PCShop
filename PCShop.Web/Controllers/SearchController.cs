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
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            this._searchService = searchService;
            this._logger = logger;
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
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
