using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Data.Models.Enum;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.Controllers;
using PCShop.Web.ViewModels.Manager;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.GCommon.MessageConstants.ManagerDashboard;

namespace PCShop.Web.Areas.Manager.Controllers
{
    [Area(ManagerRoleName)]
    [Authorize(Roles = ManagerRoleName)]
    public class OrderManagementController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderManagementController> _logger;

        public OrderManagementController(
            IOrderService orderService, 
            ILogger<OrderManagementController> logger)
        {
            this._orderService = orderService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? statusFilter, int currentPage = 1)
        {
            try
            {
                const int pageSize = 10;
                ManagerOrdersPageViewModel model;

                if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse(statusFilter, out OrderStatus statusEnum))
                {
                    model = await this._orderService.GetOrdersByStatusPagedAsync(statusEnum, currentPage, pageSize);
                    model.CurrentStatusFilter = statusFilter;
                }
                else
                {
                    model = await this._orderService.GetAllOrdersPagedAsync(currentPage, pageSize);
                    model.CurrentStatusFilter = null;
                }

                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(ManagerDashboard.LoadOrdersError, ex.Message));
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string orderId)
        {
            try
            {
                bool isApproved = await this._orderService.ApproveOrderAsync(orderId);

                if (isApproved)
                {
                    TempData["SuccessMessage"] = OrderApprovedSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = OrderApproveFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(ManagerDashboard.ApproveOrderError, orderId, ex.Message));
                TempData["ErrorMessage"] = OrderApproveFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string orderId)
        {
            try
            {
                bool isDeleted = await this._orderService.DeleteOrderAsync(orderId);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = DeleteOrderSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = DeleteOrderFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(ManagerDashboard.DeleteOrderError, orderId, ex.Message));
                TempData["ErrorMessage"] = DeleteOrderFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

    }
}
