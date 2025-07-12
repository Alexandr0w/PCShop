namespace PCShop.Web.ViewModels.Search
{
    public class SearchResultsViewModel
    {
        public required string Query { get; set; } 

        public IEnumerable<ProductSearchResultViewModel> Products { get; set; } = new List<ProductSearchResultViewModel>();

        public IEnumerable<ComputerSearchResultViewModel> Computers { get; set; } = new List<ComputerSearchResultViewModel>();
    }
}
