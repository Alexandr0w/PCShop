using static PCShop.Data.Common.EntityConstants.ApplicationUser;

namespace PCShop.Web.ViewModels.Admin.UserManagement
{
    public class UserManagementPageViewModel
    {
        public int CurrentPage { get; set; } = CurrentPageNumber;
        public int UsersPerPage { get; set; } = MaxUsersPerPage;
        public int TotalUsers { get; set; }

        public IEnumerable<UserManagementIndexViewModel> Users { get; set; } = new List<UserManagementIndexViewModel>();
    }
}
