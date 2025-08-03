using PCShop.Web.ViewModels.Search;
using static PCShop.Data.Common.EntityConstants.Search;

namespace PCShop.Services.Core.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultsViewModel> SearchAsync(string query, int currentPage = SearchCurrentPage, int itemsPerPage = SearchItemsPerPage);
    }
}
