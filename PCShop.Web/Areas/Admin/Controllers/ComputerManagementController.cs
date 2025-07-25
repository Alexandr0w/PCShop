using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.ComputerManagement;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.GCommon.MessageConstants.ComputerMessages;

namespace PCShop.Web.Areas.Admin.Controllers
{
    public class ComputerManagementController : BaseAdminController
    {
        private IComputerManagementService _computerManagementService;
        private ILogger<ComputerManagementController> _logger;

        public ComputerManagementController(IComputerManagementService computerManagementService, ILogger<ComputerManagementController> logger)
        {
            this._computerManagementService = computerManagementService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int currentPage = 1)
        {
            int computersPerPage = 10;

            IEnumerable<ComputerManagementIndexViewModel> allComputers = await this._computerManagementService
                .GetAllComputersAsync(includeDeleted: true);

            ICollection<ComputerManagementIndexViewModel> pagedComputers = allComputers
                .Skip((currentPage - 1) * computersPerPage)
                .Take(computersPerPage)
                .ToList();

            ComputerManagementPageViewModel model = new ComputerManagementPageViewModel
            {
                Computers = pagedComputers,
                TotalComputers = allComputers.Count(),
                ComputersPerPage = computersPerPage,
                CurrentPage = currentPage
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                ComputerManagementFormInputModel addComputerInputModel = new ComputerManagementFormInputModel
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
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ComputerManagementFormInputModel inputModel, IFormFile? imageFile)
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

                bool addResult = await this._computerManagementService.AddComputerAsync(userId, inputModel, imageFile);

                if (!addResult)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Computer.AddError, inputModel.Name));
                    this._logger.LogError(string.Format(Computer.AddError, inputModel.Name));

                    TempData["ErrorMessage"] = AddFailed;
                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = AddedSuccessfully;

                return this.RedirectToAction(nameof(Manage));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                this._logger.LogError(ex, InvalidFileTypeMessage);
                
                TempData["ErrorMessage"] = AddFailed;
                return this.View(inputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.AddError, ex.Message));
                TempData["ErrorMessage"] = AddFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.RedirectToAction(nameof(Manage));
                }

                ComputerManagementFormInputModel? editComputerInputModel = await this._computerManagementService.GetComputerEditFormModelAsync(id);

                if (editComputerInputModel == null)
                {
                    return this.NotFound();
                }

                return this.View(editComputerInputModel);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(LoadPage.EditPage, ex.Message));
                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ComputerManagementFormInputModel inputModel)
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
                        string imageUrl = await this._computerManagementService.UploadImageAsync(inputModel, inputModel.ImageFile);
                        inputModel.ImageUrl = imageUrl;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(inputModel.ImageFile), ex.Message);
                    this._logger.LogError(ex, ex.Message);

                    return this.View(inputModel);
                }

                bool editResult = await this._computerManagementService.EditComputerAsync(userId, inputModel, null);

                if (!editResult)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Computer.EditError, inputModel.Name));
                    this._logger.LogError(string.Format(Computer.EditError, inputModel.Name));

                    TempData["ErrorMessage"] = EditFailed;
                    return this.View(inputModel);
                }

                TempData["SuccessMessage"] = EditSuccessfully;

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.EditError, ex.Message));
                TempData["ErrorMessage"] = EditFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool result = await this._computerManagementService.SoftDeleteComputerAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = DeletedSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = DeleteFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.SoftDeleteError, ex.Message));
                TempData["ErrorMessage"] = DeleteFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidComputerId;
                    return this.RedirectToAction(nameof(Manage));
                }

                bool result = await this._computerManagementService.RestoreComputerAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ComputerRestoredSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = ComputerRestoredFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.RestoreComputerError, id, ex.Message));
                TempData["ErrorMessage"] = ComputerRestoredFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForever(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    TempData["ErrorMessage"] = InvalidComputerId;
                    return this.RedirectToAction(nameof(Manage));
                }

                bool result = await this._computerManagementService.DeleteComputerPermanentlyAsync(id);

                if (result)
                {
                    TempData["SuccessMessage"] = ComputerDeletedPermanentlySuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = ComputerDeletedPermanentlyFailed;
                }

                return this.RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Computer.HardDeleteComputerError, id, ex.Message));
                TempData["ErrorMessage"] = ComputerDeletedPermanentlyFailed;

                return this.RedirectToAction(nameof(Manage));
            }
        }
    }
}
