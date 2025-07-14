namespace PCShop.Services.Common
{
    public static class ExceptionMessages
    {
        public const string InvalidIdMessage = "Either ProductId or ComputerId must be provided";
        
        public const string QuantityMustBeGreater = "Quantity must be greater than 0";
        public const string UserIdNullOrEmpty = "User ID cannot be null or empty.";
        
        public const string ComputerIdNullOrEmpty = "Computer ID cannot be null or empty.";
        public const string InvalidComputerIdFormat = "Invalid Computer ID format.";

        public const string ProductIdNullOrEmpty = "Product ID cannot be null or empty.";
        public const string InvalidProductIdFormat = "Invalid Product ID format.";
        public const string InvalidProductTypeIdFormat = "Invalid ProductType ID format.";

        public const string InvalidFileType = "Invalid file type. Only image files are allowed.";
        public const string InvalidContentType = "Invalid content type.";
    }
}
