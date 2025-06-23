using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PCShop.Data
{
    public class PCShopDbContext : IdentityDbContext
    {
        public PCShopDbContext(DbContextOptions<PCShopDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
