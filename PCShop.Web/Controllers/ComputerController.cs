using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.Data.Common.ErrorMessages;

namespace PCShop.Web.Controllers
{
    public class ComputerController : BaseController
    {
        private readonly IComputerService _computerService;

        public ComputerController(IComputerService computerService)
        {
            this._computerService = computerService;
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
                Console.WriteLine(e.Message);
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

                if (userId == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                DetailsComputerViewModel? computerDetails = await this._computerService.GetComputerDetailsAsync(userId, id);

                if (computerDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(computerDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                Console.WriteLine(e.Message);
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
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
