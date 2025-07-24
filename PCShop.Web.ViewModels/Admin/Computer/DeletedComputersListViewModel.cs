namespace PCShop.Web.ViewModels.Admin.Computer
{
    public class DeletedComputersListViewModel
    {
        public int CurrentPage { get; set; }

        public int ProductsPerPage { get; set; }

        public int TotalProducts { get; set; }

        public IEnumerable<DeletedComputerViewModel> Computers { get; set; } = new List<DeletedComputerViewModel>();
    }
}
