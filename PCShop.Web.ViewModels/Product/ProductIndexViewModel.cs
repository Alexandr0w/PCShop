namespace PCShop.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public int Id { get; set; }

        public required string Title { get; set; } 

        public required string ImageUrl { get; set; }

        public required string Category { get; set; } 

        public bool IsAuthor { get; set; }

        public bool IsSaved { get; set; }

        public int SavedCount { get; set; }
    }
}
