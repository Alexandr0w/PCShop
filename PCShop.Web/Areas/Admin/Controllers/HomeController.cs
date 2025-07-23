using Microsoft.AspNetCore.Mvc;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}