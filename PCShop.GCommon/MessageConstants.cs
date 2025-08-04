using System.Data;

namespace PCShop.GCommon
{
    public static class MessageConstants
    {
        public static class ProductMessages
        {
            public const string AddedSuccessfully = "Product added successfully.";
            public const string AddFailed = "Failed to add the product. Please try again.";

            public const string EditedSuccessfully = "Product updated successfully.";
            public const string EditeFailed = "Failed to update the product. Please try again.";

            public const string DeletedSuccessfully = "Product deleted successfully.";
            public const string DeleteFailed = "Failed to delete the product. Please try again.";

            public const string DeletedProductsFailed = "Error loading deleted products.";
            public const string InvalidProductId = "Missing product ID.";

            public const string ProductRestoredSuccessfully = "Product restored successfully.";
            public const string ProductRestoredFailed = "Failed to restore product. It may not exist or is already active.";
            public const string ProductRestoredError = "An error occurred while restoring the product.";

            public const string ProductDeletedPermanentlySuccessfully = "Product permanently deleted.";
            public const string ProductDeletedPermanentlyFailed
                = "Failed to permanently delete product. Product may not exist or is not deleted.";

            public const string HardDeleteProductError = "An error occurred while restoring the product.";
        }

        public static class ComputerMessages
        {
            public const string AddedSuccessfully = "Computer added successfully.";
            public const string AddFailed = "Failed to add the computer. Please try again.";

            public const string EditSuccessfully = "Computer updated successfully.";
            public const string EditFailed = "Failed to update the computer. Please try again.";

            public const string DeletedSuccessfully = "Computer deleted successfully.";
            public const string DeleteFailed = "Failed to delete the computer. Please try again.";

            public const string DeletedComputersFailed = "Error loading deleted computers.";
            public const string InvalidComputerId = "Missing computer ID.";

            public const string ComputerRestoredSuccessfully = "Computer restored successfully.";
            public const string ComputerRestoredFailed = "Failed to restore computer. It may not exist or is already active.";
            public const string ComputerRestoredError = "An error occurred while restoring the computer.";

            public const string ComputerDeletedPermanentlySuccessfully = "Computer permanently deleted.";
            public const string ComputerDeletedPermanentlyFailed
                = "Failed to permanently delete computer. Computer may not exist or is not deleted.";

            public const string HardDeleteComputerError = "An error occurred while restoring the computer.";
        }

        public static class OrderMessages
        {
            public const string AddedToCartSuccessfully = "Product added to cart successfully.";
            public const string AddToCartFailed = "Failed to add product to cart. Please try again.";

            public const string ItemRemovedSuccessfully = "Item removed from cart successfully.";
            public const string RemoveItemFailed = "Failed to remove item from cart. Please try again.";

            public const string ClearedSuccessfully = "Cart cleared successfully.";
            public const string ClearCartFailed = "Failed to clear cart. Please try again.";

            public const string FinalizedSuccessfully = "Order finalized successfully!";
            public const string FinalizeFailed = "Failed to finalize order. Please try again.";

            public const string CompleteProfile = "Please complete your profile before placing an order.";
            public const string NoPendingOrders = "No pending order found or cart is empty.";
            public const string UnableToConfirm = "Unable to confirm order. Please try again.";

            public const string ConfirmedSuccessfully = "Order confirmed successfully!";
            public const string ConfirmationFailed = "Failed to confirm order. Please try again.";

            public const string PaymentSuccessfully = "Payment successfully! Your order will be confirmed soon.";
            public const string PaymentFailed = "Payment failed. Please try again.";
            public const string OrderDataNotFound = "Order data not found. Please try again or contact support.";
            public const string PaymentCanceled = "Payment has been canceled. Please try again.";
            public const string PaymentError = "Failed to processing the payment. Please try again.";
            public const string PaymentCancelFailed = "Failed to cancel the payment. Please try again.";
        }

        public static class UserManagement
        {
            public const string InvalidUserId = "Invalid user ID.";
            public const string UserNotFound = "User not found.";

            public const string AssignRoleSuccessfully = "Role '{0}' assigned successfully.";
            public const string AssignRoleFailed = "Failed to assign role '{0}'.";

            public const string RemoveRoleSuccessfully = "Role '{0}' removed successfully.";
            public const string RemoveRoleFailed = "Failed to remove role '{0}'.";

            public const string DeletedUserSuccessfully = "User soft-deleted successfully.";
            public const string DeleteUserFailed = "Failed to soft-delete user.";

            public const string RestoreUserSuccessfully = "User restored successfully.";
            public const string RestoreUserFailed = "Failed to restore user. User may not exist or is already active.";

            public const string DeleteUserPermanentlySuccessfully = "User permanently deleted.";
            public const string DeleteUserPermanentlyFailed = "Failed to permanently delete user. User may not exist or is not deleted.";
        }

        public static class ManagerDashboard
        {
            public const string OrderApprovedSuccessfully = "Order approved successfully.";
            public const string OrderApproveFailed = "Failed to approve order. Please try again.";

            public const string DeleteOrderSuccessfully = "Order deleted successfully.";
            public const string DeleteOrderFailed = "Failed to delete order. Please try again.";
        }

        public static class NotificationMessages
        {
            public const string InvalidBulkAction = "Invalid bulk action.";
            public const string BulkActionError = "Bulk action failed.";
            public const string NoNotificationsSelected = "No notifications selected.";

            public const string NotificationsMarkedAsRead = "{0} notifications marked as read.";
            public const string NotificationsDeleted = "{0} notifications deleted.";

            public const string NotificationDeletedSuccessfully = "Notification deleted successfully.";
            public const string NotificationDeleteFailed = "Failed to delete notification. Please try again.";
        }
    }
}
