using static PCShop.Data.Common.EntityConstants.Computer;
namespace PCShop.Web.ViewModels.Admin.ComputerManagement
{
    public class ComputerManagementPageViewModel
    {
        public int CurrentPage { get; set; } = CurrentPageNumber;

        public int ComputersPerPage { get; set; } = MaxComputersPerPage;

        public int TotalComputers { get; set; }

        public IEnumerable<ComputerManagementIndexViewModel> Computers { get; set; } = new List<ComputerManagementIndexViewModel>();
    }
}
