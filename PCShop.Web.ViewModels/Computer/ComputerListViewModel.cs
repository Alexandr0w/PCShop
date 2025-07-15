using static PCShop.Data.Common.EntityConstants.Computer;

namespace PCShop.Web.ViewModels.Computer
{
    public class ComputerListViewModel
    {
        public int CurrentPage { get; set; } = CurrentPageNumber;

        public int ComputersPerPage { get; set; } = MaxComputersPerPage;

        public int TotalComputers { get; set; }

        public string? SearchTerm { get; set; }

        public string? SortOption { get; set; }

        public IEnumerable<ComputerIndexViewModel> Computers { get; set; } = new List<ComputerIndexViewModel>();
    }
}
