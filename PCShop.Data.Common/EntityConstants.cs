namespace PCShop.Data.Common
{
    public static class EntityConstants
    {
        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 50;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 50;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 100;

            public const int CityMinLength = 2;
            public const int CityMaxLength = 50;

            public const int PostalCodeMinLength = 2;
            public const int PostalCodeMaxLength = 10;
        }

        public static class Computer
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 1000;

            public const string PriceType = "decimal(18,2)";

            public const int ImageUrlMaxLength = 2048;
        }

        public static class Product
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 1000;

            public const string PriceType = "decimal(18,2)";

            public const int ImageUrlMaxLength = 2048;
        }

        public static class ProductType
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public static class Order
        {
            public const string TotalPriceType = "decimal(18,2)";
            public const string OrderDateDefaultValue = "GETUTCDATE()";
        }
    }
}
