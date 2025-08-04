using System.IO.Pipes;

namespace PCShop.GCommon
{
    public static class ErrorMessages
    {
        public static class Common
        {
            public const string ModificationNotAllowed = "Modification is not allowed.";
            public const string RequiredId = "Product ID or Computer ID is required.";
            public const string QuantityGreaterThanZero = "Quantity must be greater than 0.";
            public const string ExceptionOccurred = "An unhandled exception occurred";
        }

        public static class LoadPage
        {
            public const string IndexError = "An error occurred in the Index page: {0}.";
            public const string DetailsError = "An error occurred in the Details page: {0}.";
            public const string AddPage = "An error occurred while loading the Add page: {0}.";
            public const string EditPage = "An error occurred while loading the Edit page: {0}.";
            public const string DeletePage = "An error occurred while loading the Delete page: {0}.";
        }

        public static class Search
        {
            public const string QueryError = "An error occurred while processing the search query: {0}.";
        }

        public static class Product
        {
            public const string AddError = "An error occurred while adding the product: {0}.";
            public const string EditError = "An error occurred while updating the product: {0}.";
            public const string DeleteError = "An error occurred while deleting the product: {0}.";

            public const string SoftDeleteError = "An error occired while soft-delete the product: {0}.";
            public const string RestoreProductError = "An error occurred while restoring the product '{0}': {1}.";
            public const string HardDeleteProductError = "An error occurred while permanently deleting the product '{0}': {1}.";
        }

        public static class Computer
        {
            public const string AddError = "An error occurred while adding the computer: {0}.";
            public const string EditError = "An error occurred while updating the computer: {0}.";
            public const string DeleteError = "An error occurred while deleting the computer: {0}.";

            public const string SoftDeleteError = "An error occired while soft-delete the computer: {0}.";
            public const string RestoreComputerError = "An error occurred while restoring the computer '{0}': {1}.";
            public const string HardDeleteComputerError = "An error occurred while permanently deleting the computer '{0}': {1}.";
        }

        public static class Order
        {
            public const string AddError = "An error occurred while adding product '{0}' to the cart.";
            public const string RemoveError = "An error occurred while removing product '{0}' from the cart.";

            public const string ClearError = "An error occurred while clearing the cart: {0}.";
            public const string FinalizeError = "An error occurred while finalizing the order: {0}.";

            public const string ConfirmError = "An error occurred while confirming the order: {0}.";
            public const string LoadConfirmationError = "An error occurred while loading the confirmation: {0}.";

            public const string DeliveryMethodError = "Please select a delivery method.";
            public const string PaymentMethodError = "Invalid payment method selected.";

            public const string StripeSettingsError = "Stripe settings are not configured correctly. Please check the configuration.";
            public const string SuccessPaymentError = "Error processing successful payment: {0}.";
            public const string FailedPaymentError = "Error processing failed payment: {0}.";
            public const string PaymentCanceledError = "Payment was canceled by the user: {0}.";

            public const string CartCountError = "Error in cart count get method {0}.";
        }

        public static class UserManagement
        {
            public const string IndexError = "An error occurred while loading the user management page: {0}.";
            public const string AssignRoleError = "An error occurred while assigning the role '{1}' to user '{0}': {2}.";
            public const string RemoveRoleError = "An error occurred while removing the role '{1}' from user '{0}': {2}.";
            public const string DeleteUserError = "An error occurred while deleting the user '{0}': {1}.";
            public const string RestoreUserError = "An error occurred while restoring the user '{0}': {1}.";
            public const string DeleteUserForeverError = "An error occurred while permanently deleting the user '{0}': {1}.";
        }

        public static class ManagerDashboard
        {
            public const string LoadOrdersError = "An error occurred while loading orders: {0}.";
            public const string ApproveOrderError = "An error occurred while approving the order '{0}': {1}.";
            public const string DeleteOrderError = "An error occurred while deleting the order '{0}': {1}.";
        }

        public static class Notification
        {
            public const string LoadIndexError = "An error occurred while loading user notifications: {0}.";
            public const string GetUnreadCountError = "An error occurred while getting unread notifications count: {0}.";
            public const string DeleteNotificationError = "An error occurred while deleting notification: {0}.";
            public const string BulkActionError = "Bulk action failed: {0}.";
        }
    }
}
