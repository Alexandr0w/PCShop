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
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();

                IEnumerable<ComputerIndexViewModel> allComputers = await this._computerService.GetAllComputersAsync(userId);

                return this.View(allComputers);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
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
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(computerDetails);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            try
            {
                AddComputerInputModel addComputerInputModel = new AddComputerInputModel
                {
                    Name = string.Empty,
                    Description = string.Empty,
                    ImageUrl = string.Empty
                };

                return this.View(addComputerInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddComputerInputModel inputModel, IFormFile? imageFile)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool addResult = await this._computerService.AddComputerAsync(this.GetUserId(), inputModel, imageFile);

                if (addResult == false)
                {
                    ModelState.AddModelError(string.Empty, AddProductErrorMessage);
                    this._logger.LogError(AddProductErrorMessage);

                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
