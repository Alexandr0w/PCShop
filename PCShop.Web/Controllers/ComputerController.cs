using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ErrorMessages;

namespace PCShop.Web.Controllers
{
    public class ComputerController : BaseController
    {
        private readonly IComputerService _computerService;
        private readonly ILogger<ComputerController> _logger;

        public ComputerController(IComputerService computerService, ILogger<ComputerController> logger)
        {
            this._computerService = computerService;
            this._logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] ComputerListViewModel model)
        {
            try
            {
                string? userId = this.GetUserId();

                await this._computerService.GetAllComputersQueryAsync(model);
                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.IndexError, ex.Message));
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                string? userId = this.GetUserId();

                DetailsComputerViewModel? computerDetails = await this._computerService.GetComputerDetailsAsync(userId, id);

                if (computerDetails == null)
                {
                    return this.NotFound();
                }

                return this.View(computerDetails);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.DetailsError, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
