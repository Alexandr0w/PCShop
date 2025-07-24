using PCShop.Data.Models.Enum;

namespace PCShop.Web.ViewModels.Manager
{
    public class ManagerOrdersPageViewModel
    {
        public IEnumerable<ManagerOrderViewModel> Orders { get; set; } = new List<ManagerOrderViewModel>();

        public int CurrentPage { get; set; }

        public int TotalOrders { get; set; }

        public int OrdersPerPage { get; set; } = 10;

        public string? CurrentStatusFilter { get; set; }

        public List<string> AllStatuses { get; set; } = Enum.GetNames(typeof(OrderStatus)).ToList();

        public int TotalPages { get; set; }
    }
}
