using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Notification;
using static PCShop.GCommon.ErrorMessages;
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

                NotificationListViewModel model = await this._notificationService.GetUserNotificationsAsync(userId, currentPage, 10);
                return this.View(model);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.LoadIndexError, ex.Message));
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            try
            {
                bool isMarked = await this._notificationService.MarkAsReadAsync(id);

                if (isMarked)
                {
                    TempData["SuccessMessage"] = MarkedAsReadSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = MarkedAsReadFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.MarkAsReadError, ex.Message));
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                string? userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized();
                }

                bool allMarked = await this._notificationService.MarkAllAsReadAsync(userId);

                if (allMarked)
                {
                    TempData["SuccessMessage"] = MarkedAllAsReadSuccessfully;
                }
                else
                {
                    TempData["ErrorMessage"] = MarkedAllAsReadFailed;
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(Notification.MarkAllAsReadError, ex.Message));
                return this.RedirectToAction(nameof(Index));
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
                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}