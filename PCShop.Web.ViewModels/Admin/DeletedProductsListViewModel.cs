namespace PCShop.Web.ViewModels.Admin
{
    public class DeletedProductsListViewModel
    {
        public int CurrentPage { get; set; }

        public int ProductsPerPage { get; set; }

        public int TotalProducts { get; set; }

        public IEnumerable<DeletedProductViewModel> Products { get; set; } = new List<DeletedProductViewModel>();
    }
}
