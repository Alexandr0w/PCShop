using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Data.Seeding;
using PCShop.Data.Seeding.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.Infrastructure.Emailing;
using PCShop.Web.Infrastructure.Extensions;
using PCShop.Web.Infrastructure.Middlewares;
using PCShop.Web.Infrastructure.Settings;

namespace PCShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("PCShopDbConnection")
                ?? throw new InvalidOperationException("Connection string 'PCShopDbConnection' not found.");

            builder.Services
                .AddDbContext<PCShopDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.ConfigureFromConfiguration(builder.Configuration);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PCShopDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";

                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            // Loading all services and repositories with extension method
            builder.Services.AddRepositories(typeof(IProductRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IProductService).Assembly);

            // Seeding roles for Identity
            builder.Services.AddTransient<IIdentityDbSeeder, IdentityDbSeeder>();

            // Configuring email sender service 
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Configuring Stripe (payment with credit card)
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            // Configuring session for storing cart items
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120); 
                options.Cookie.HttpOnly = true;                  
                options.Cookie.IsEssential = true;               
            });

            builder.Services.AddControllersWithViews(options =>
            {
                // Adding AutoValidateAntiforgeryTokenAttribute globally
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddRazorPages();

            WebApplication app = builder.Build();

            // Creating Default Admin, Manager, and User roles
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IIdentityDbSeeder seeder = scope.ServiceProvider.GetRequiredService<IIdentityDbSeeder>();
                seeder.SeedAsync().GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseGlobalExceptionHandling();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Added custom Error pages
            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseAuthentication();
            app.UseAuthorization();

            // Using session for storing cart items
            app.UseSession();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}
