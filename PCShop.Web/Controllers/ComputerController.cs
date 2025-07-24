using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.GCommon.MessageConstants.ComputerMessages;

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

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
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
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.AddPage, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Add(ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (!this.ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                bool addResult = await this._computerService.AddComputerAsync(userId, inputModel, imageFile);

                if (!addResult)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Computer.AddError, inputModel.Name));
                    this._logger.LogError(string.Format(Computer.AddError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = AddedSuccessfully;

                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                this._logger.LogError(ex, InvalidFileTypeMessage);

                return this.View(inputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.AddError, ex.Message));
                TempData["ErrorMessage"] = AddFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
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
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.EditPage, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public async Task<IActionResult> Edit(ComputerFormInputModel inputModel)
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                if (!this.ModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                try
                {
                    if (inputModel.ImageFile != null && inputModel.ImageFile.Length > 0)
                    {
                        string imageUrl = await this._computerService.UploadImageAsync(inputModel, inputModel.ImageFile);
                        inputModel.ImageUrl = imageUrl;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                    this._logger.LogError(ex, ex.Message);

                    return this.View(inputModel);
                }

                bool editResult = await this._computerService.PersistUpdatedComputerAsync(userId, inputModel, null);

                if (!editResult)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Computer.EditError, inputModel.Name));
                    this._logger.LogError(string.Format(Computer.EditError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = UpdatedSuccessfully;

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.EditError, ex.Message));
                TempData["ErrorMessage"] = UpdateFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = AdminRoleName)]
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
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.DeletePage, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
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
                    ModelState.AddModelError(string.Empty, Common.ModificationNotAllowed);
                    this._logger.LogError(Common.ModificationNotAllowed);

                    return this.View(inputModel);
                }

                bool deleteResult = await this._computerService.SoftDeleteComputerAsync(userId, inputModel);

                if (!deleteResult)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Computer.DeleteError, inputModel.Name));
                    this._logger.LogError(string.Format(Computer.DeleteError, inputModel.Name));

                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = DeletedSuccessfully;

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.DeleteError, ex.Message));
                TempData["ErrorMessage"] = DeleteFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
