using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Notification;
using static PCShop.GCommon.ErrorMessages;
using static PCShop.Services.Common.ServiceConstants;
using static PCShop.GCommon.MessageConstants.NotificationMessages;

namespace PCShop.Web.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            this._notificationService = notificationService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index(int currentPage = 1)
        {
            try
            {
                string? userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                bool hasUnread = await this._notificationService.HasUnreadNotificationsAsync(userId);
                ViewBag.HasUnreadNotifications = hasUnread;

                NotificationListViewModel model = await this._notificationService.GetUserNotificationsAsync(userId, currentPage, NotificationsPageSize);
                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.LoadIndexError, ex.Message));
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                string? userId = this.GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Json(new { unreadCount = 0 });
                }

                int count = await this._notificationService.GetUnreadCountAsync(userId);
                return this.Json(new { unreadCount = count });
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.GetUnreadCountError, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool isDeleted = await this._notificationService.DeleteNotificationAsync(id);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = NotificationDeletedSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = NotificationDeleteFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.DeleteNotificationError, ex.Message));
                TempData["ErrorMessage"] = NotificationDeleteFailed;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> BulkAction(string[] selectedIds, string action)
        {
            try
            {
                if (selectedIds == null || selectedIds.Length == 0)
                {
                    TempData["ErrorMessage"] = NoNotificationsSelected;
                    return this.RedirectToAction(nameof(Index));
                }

                if (action == "markAsRead")
                {
                    int markedCount = await this._notificationService.MarkMultipleAsReadAsync(selectedIds);
                    TempData["SuccessMessage"] = string.Format(NotificationsMarkedAsRead, markedCount);
                }
                else if (action == "delete")
                {
                    int deletedCount = await this._notificationService.DeleteMultipleAsync(selectedIds);
                    TempData["SuccessMessage"] = string.Format(NotificationsDeleted, deletedCount);
                }
                else
                {
                    TempData["ErrorMessage"] = InvalidBulkAction;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.BulkActionError, ex.Message));
                TempData["ErrorMessage"] = BulkActionError;

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}