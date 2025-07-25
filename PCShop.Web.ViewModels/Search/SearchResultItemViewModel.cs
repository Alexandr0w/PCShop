namespace PCShop.Web.ViewModels.Search
{
    public class SearchResultItemViewModel
    {
        public required string Id { get; set; } 
        public required string Name { get; set; } 
        public decimal Price { get; set; }
        public required string Type { get; set; } 
    }
}
