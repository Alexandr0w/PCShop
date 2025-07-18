namespace PCShop.GCommon
{
    public static class ExceptionMessages
    {
        public const string InterfaceNotFoundMessage =
            "The {0} could not be added to the Service Collection, because no interface matching the convention could be found! Convention for Interface naming: I{0}.";

        public const string InvalidIdMessage = "Either ProductId or ComputerId must be provided";

        public const string QuantityMustBeGreaterMessage = "Quantity must be greater than 0";
        public const string UserIdNullOrEmptyMessage = "User ID cannot be null or empty.";

        public const string ComputerIdNullOrEmptyMessage = "Computer ID cannot be null or empty.";
        public const string InvalidComputerIdFormatMessage = "Invalid Computer ID format.";

        public const string ProductIdNullOrEmptyMessage = "Product ID cannot be null or empty.";
        public const string InvalidProductIdFormatMessage = "Invalid Product ID format.";
        public const string InvalidProductTypeIdFormatMessage = "Invalid ProductType ID format.";

        public const string InvalidFileTypeMessage = "Invalid file type. Only image files are allowed.";
        public const string InvalidContentTypeMessage = "Invalid content type.";
        public const string ImageNotFoundMessage = "Please provide a valid image file or image URL.";
        public const string InvalidImageUrlFormatMessage = "Invalid image URL format.";
        public const string InvalidOrUnsafeImageUrlMessage = "Invalid or unsafe image URL.";

        public const string UnknownDeliveryMethodMessage = "Unknown delivery method.";
    }
}
