using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using PCShop.Web.ViewModels.Product;
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

                await this._computerService.PopulateComputerQueryModelAsync(model, userId);
                return this.View(model);
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
                    return this.NotFound();
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
                ComputerFormInputModel addComputerInputModel = new ComputerFormInputModel
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
        public async Task<IActionResult> Add(ComputerFormInputModel inputModel, IFormFile? imageFile)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.RedirectToAction(nameof(Index));
                }

                ComputerFormInputModel? editComputerInputModel = await this._computerService.GetComputerForEditingAsync(id);

                if (editComputerInputModel == null)
                {
                    return this.NotFound();
                }

                return this.View(editComputerInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                bool editResult = await this._computerService.PersistUpdatedComputerAsync(userId, inputModel, imageFile);

                if (editResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, EditComputerErrorMessage);
                    this._logger.LogError(EditComputerErrorMessage);

                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                string? userId = this.GetUserId();

                DeleteComputerViewModel? deleteComputerInputModel = await this._computerService.GetComputerForDeletingAsync(userId, id);

                if (deleteComputerInputModel == null)
                {
                    return this.NotFound();
                }

                return this.View(deleteComputerInputModel);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteComputerViewModel inputModel)
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, NotModifyMessage);
                    this._logger.LogError(NotModifyMessage);

                    return this.View(inputModel);
                }

                bool deleteResult = await this._computerService.SoftDeleteComputerAsync(userId, inputModel);

                if (deleteResult == false)
                {
                    ModelState.AddModelError(string.Empty, DeleteComputerErrorMessage);
                    this._logger.LogError(DeleteComputerErrorMessage);

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
