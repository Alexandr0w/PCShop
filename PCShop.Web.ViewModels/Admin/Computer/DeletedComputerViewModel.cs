namespace PCShop.Web.ViewModels.Admin.Computer
{
    public class DeletedComputerViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
