using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Web.ViewModels;
using System.Diagnostics;

namespace PCShop.Web.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                if (IsUserAuthenticated())
                {
                    return this.RedirectToAction(nameof(Index), "Product");
                }

                return this.View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}