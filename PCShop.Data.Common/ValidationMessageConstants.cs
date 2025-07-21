using System.Numerics;

namespace PCShop.Data.Common
{
    public static class ValidationMessageConstants
    {
        public static class Common
        {
            public const string NameRequired = "{0} is required.";
            public const string NameLength = "{0} must be between {2} and {1} characters long.";

            public const string DescriptionRequired = "{0} is required.";
            public const string DescriptionLength = "{0} must be between {2} and {1} characters long.";

            public const string PriceRequired = "{0} is required.";
            public const string PriceRange = "{0} must be between {1} and {2}.";

            public const string CreatedOnRequired = "{0} is required.";
            public const string CreatedOnNeededLength = "{0} must be exactly {1} characters long.";

            public const string ImageUrlLength = "{0} must not exceed {1} characters.";

            public const string ProductTypeRequired = "{0} is required.";
            public const string ComputerTypeRequired = "{0} is required.";
        }

        public static class ApplicationUser
        {
            public const string FullNameRequired = "{0} is required.";
            public const string FullNameLength = "{0} must be between {2} and {1} characters long.";

            public const string AddressRequired = "{0} is required.";
            public const string AddressLength = "{0} must be between {2} and {1} characters long.";

            public const string CityRequired = "{0} is required.";
            public const string CityLength = "{0} must be between {2} and {1} characters long.";

            public const string PostalCodeRequired = "{0} is required.";
            public const string PostalCodeLength = "{0} must be between {2} and {1} characters long.";

            public const string PhoneNumberRequired = "{0} is required.";
            public const string PhoneNumberInvalid = "{0} is not a valid phone number.";

            public const string EmailRequired = "{0} is required.";
            public const string EmailInvalid = "{0} is not a valid email address.";
        }

        public static class Order
        {
            public const string DeliveryMethodRequired = "Please select a delivery method.";
            public const string DeliveryMethodInvalid = "Invalid delivery method.";
        }
    }
}
