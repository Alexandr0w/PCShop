namespace PCShop.GCommon
{
    public static class ApplicationConstants
    {
        public const string AppDateFormat = "yyyy-MM-dd";
        public const string OrderDateFormat = "dd.MM.yyyy HH:mm";

        public const string NoImageUrl = "no-image.jpg";
        public const string PriceSqlType = "decimal(18,2)";
        public const string OrderDateDefaultSqlValue = "GETUTCDATE()";
        public const string IsDeletedPropertyName = "IsDeleted";

        public const string AdminRoleName = "Admin";
        public const string ManagerRoleName = "Manager";
        public const string UserRoleName = "User";

        public const string AdminUserName = "admin@pcshop.com";
        public const string AdminPassword = "Admin123!";

        public const string ManagerUserName = "manager@pcshop.com";
        public const string ManagerPassword = "Manager123!";

        public const string DefaultUserName = "user@pcshop.com";
        public const string DefaultUserPassword = "User123!";

        public const string Unauthorized403Path = "/Home/Error?statusCode=403";
        public const string NotFound404Path = "/Home/Error?statusCode=404";
        public const string InternalServerError500Path = "/Home/Error?statusCode=500";
    }
}
