using PCShop.Data.Models;
using PCShop.Web.ViewModels.Search;

namespace PCShop.Services.Core.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultsViewModel> SearchAsync(string query);
    }
}
