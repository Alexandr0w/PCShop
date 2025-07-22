using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace PCShop.Web.Infrastructure.Extensions
{
    public static class IdentityOptionsExtensions
    {
        public static void ConfigureFromConfiguration(this IdentityOptions options, IConfiguration configuration)
        {
            options.SignIn.RequireConfirmedEmail =
                configuration.GetValue<bool>("IdentityConfig:SignIn:RequireConfirmedEmail");

            options.SignIn.RequireConfirmedAccount =
                configuration.GetValue<bool>("IdentityConfig:SignIn:RequireConfirmedAccount");

            options.SignIn.RequireConfirmedPhoneNumber =
                configuration.GetValue<bool>("IdentityConfig:SignIn:RequireConfirmedPhoneNumber");

            options.Password.RequiredLength =
                configuration.GetValue<int>("IdentityConfig:Password:RequiredLength");

            options.Password.RequireNonAlphanumeric =
                configuration.GetValue<bool>("IdentityConfig:Password:RequireNonAlphanumeric");

            options.Password.RequireDigit =
                configuration.GetValue<bool>("IdentityConfig:Password:RequireDigit");

            options.Password.RequireLowercase =
                configuration.GetValue<bool>("IdentityConfig:Password:RequireLowercase");

            options.Password.RequireUppercase =
                configuration.GetValue<bool>("IdentityConfig:Password:RequireUppercase");

            options.Password.RequiredUniqueChars =
                configuration.GetValue<int>("IdentityConfig:Password:RequiredUniqueChars");
        }
    }
}
