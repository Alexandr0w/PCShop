namespace PCShop.Web.ViewModels.Computer
{
    public class DetailsComputerViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string ImageUrl { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public required string CreatedOn { get; set; }
    }
}
