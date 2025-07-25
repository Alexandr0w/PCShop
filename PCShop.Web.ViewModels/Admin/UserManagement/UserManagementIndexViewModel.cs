namespace PCShop.Web.ViewModels.Admin.UserManagement
{
    public class UserManagementIndexViewModel
    {
        public required string Id { get; set; } 

        public string? Email { get; set; }

        public bool IsDeleted { get; set; }

        public required IEnumerable<string> Roles { get; set; }
    }
}
