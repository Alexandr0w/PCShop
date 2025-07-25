namespace PCShop.Web.ViewModels.Admin.ComputerManagement
{
    public class ComputerManagementIndexViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
