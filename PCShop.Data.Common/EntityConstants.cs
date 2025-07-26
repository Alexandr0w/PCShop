namespace PCShop.Data.Common
{
    public static class EntityConstants
    {
        public static class ApplicationUser
        {
            public const int FullNameMinLength = 2;
            public const int FullNameMaxLength = 70;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 50;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 100;

            public const int CityMinLength = 2;
            public const int CityMaxLength = 50;

            public const int PostalCodeMinLength = 2;
            public const int PostalCodeMaxLength = 10;

            public const int CurrentPageNumber = 1;
            public const int MaxUsersPerPage = 10;
        }

        public static class Computer
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 1000;

            public const int CreatedOnLength = 16;

            public const int ImageUrlMaxLength = 2048;

            public const double PriceMinValue = 1;
            public const double PriceMaxValue = 10000;

            public const int CurrentPageNumber = 1;
            public const int MaxComputersPerPage = 8;

            public const string DefaultSortOption = "default";
        }

        public static class Product
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 1000;

            public const int CreatedOnLength = 16;

            public const int ImageUrlMaxLength = 2048;

            public const double PriceMinValue = 1;
            public const double PriceMaxValue = 10000;

            public const int ProductCurrentPage = 1;
            public const int MaxProductsPerPage = 12;

            public const string DefaultSortOption = "default";
        }

        public static class ProductType
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }

        public static class OrderItem
        {
            public const int QuantityDefaultValue = 1;
        }

        public static class Order
        {
            public const int CommentMinLength = 4;
            public const int CommentMaxLength = 500;

            public const int DeliveryMethodMinLength = 5;
            public const int DeliveryMethodMaxLength = 50;

            public const int DeliveryAddressMinLength = 5;
            public const int DeliveryAddressMaxLength = 300;
        }

        public static class Notification
        {
            public const int MessageMinLength = 5;
            public const int MessageMaxLength = 300;
        }

        public static class Search
        {
            public const int SearchCurrentPage = 1;
            public const int SearchProductsPerPage = 10;
        }
    }
}
