namespace PCShop.Web.ViewModels.Admin.ComputerManagement
{
    public class ComputerManagementPageViewModel
    {
        public required IEnumerable<ComputerManagementIndexViewModel> Computers { get; set; } 
        public int TotalComputers { get; set; }

        public int ComputersPerPage { get; set; }

        public int CurrentPage { get; set; }
    }
}
