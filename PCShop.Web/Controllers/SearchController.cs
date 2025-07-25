﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Search;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.Data.Common.EntityConstants.Search;

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
        public async Task<IActionResult> Index(string query, int currentPage = SearchCurrentPage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return this.View(new SearchResultsViewModel { Query = "" });
                }

                SearchResultsViewModel results = await this._searchService.SearchAsync(query, currentPage);

                return this.View(results);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Search.QueryError, ex.Message));
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
