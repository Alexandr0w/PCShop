using Microsoft.AspNetCore.Identity;

namespace PCShop.Web.ViewModels.Admin.UserManagement
{
    public class UserManagementPageViewModel
    {
        public required IEnumerable<UserManagementIndexViewModel> Users { get; set; }

        public required IEnumerable<string> Roles { get; set; } 
    }
}
