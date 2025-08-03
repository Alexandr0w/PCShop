using static PCShop.Data.Common.EntityConstants.Search;

namespace PCShop.Web.ViewModels.Search
{
    public class SearchResultsViewModel
    {
        public required string Query { get; set; }

        public int CurrentPage { get; set; } = SearchCurrentPage;

        public int ItemsPerPage { get; set; } = SearchItemsPerPage;

        public int TotalResults { get; set; }

        public IEnumerable<SearchResultItemViewModel> Results { get; set; } = new List<SearchResultItemViewModel>();
    }
}