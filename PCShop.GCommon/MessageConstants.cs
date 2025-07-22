namespace PCShop.GCommon
{
    public static class MessageConstants
    {
        public static class ProductMessages
        {
            public const string AddedSuccessfully = "Product added successfully.";
            public const string AddFailed = "Failed to add the product. Please try again.";

            public const string UpdatedSuccessfully = "Product updated successfully.";
            public const string UpdateFailed = "Failed to update the product. Please try again.";

            public const string DeletedSuccessfully = "Product deleted successfully.";
            public const string DeleteFailed = "Failed to delete the product. Please try again.";
        }

        public static class ComputerMessages
        {
            public const string AddedSuccessfully = "Computer added successfully.";
            public const string AddFailed = "Failed to add the computer. Please try again.";

            public const string UpdatedSuccessfully = "Computer updated successfully.";
            public const string UpdateFailed = "Failed to update the computer. Please try again.";

            public const string DeletedSuccessfully = "Computer deleted successfully.";
            public const string DeleteFailed = "Failed to delete the computer. Please try again.";
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

            public const string PaymentSuccessfully = "Payment successfully! Your order has been confirmed.";
            public const string PaymentFailed = "Payment failed. Please try again.";
            public const string OrderDataNotFound = "Order data not found. Please try again or contact support.";
            public const string PaymentCanceled = "Payment has been canceled. Please try again.";
            public const string PaymentError = "Failed to processing the payment. Please try again.";
            public const string PaymentCancelFailed = "Failed to cancel the payment. Please try again.";
        }
    }
}
