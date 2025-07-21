using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}