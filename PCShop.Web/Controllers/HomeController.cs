using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Web.ViewModels;
using System.Diagnostics;

namespace PCShop.Web.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 401:
                case 403:
                    return this.View("UnauthorizedError");
                case 404:
                    return this.View("NotFoundError");
                default:
                    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}