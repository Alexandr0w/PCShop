namespace PCShop.GCommon
{
    public static class ErrorMessages
    {
        public static class Common
        {
            public const string ModificationNotAllowed = "Modification is not allowed.";
            public const string RequiredId = "Product ID or Computer ID is required.";
            public const string QuantityGreaterThanZero = "Quantity must be greater than 0.";
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
        }

        public static class Computer
        {
            public const string AddError = "An error occurred while adding the computer: {0}.";
            public const string EditError = "An error occurred while updating the computer: {0}.";
            public const string DeleteError = "An error occurred while deleting the computer: {0}.";
        }

        public static class Order
        {
            public const string AddError = "An error occurred while adding product '{0}' to the cart.";
            public const string RemoveError = "An error occurred while removing product '{0}' from the cart.";
            public const string ClearError = "An error occurred while clearing the cart: {0}.";
            public const string FinalizeError = "An error occurred while finalizing the order: {0}.";
            public const string ConfirmError = "An error occurred while confirming the order: {0}.";
            public const string LoadConfirmationError = "An error occurred while loading the confirmation: {0}.";
        }
    }
}
