using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.ApplicationUser;

namespace PCShop.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity
                .Property(au => au.FullName)
                .IsRequired()
                .HasMaxLength(FullNameMaxLength);

            entity
                .Property(au => au.Address)
                .IsRequired()
                .HasMaxLength(AddressMaxLength);

            entity
                .Property(au => au.City)
                .IsRequired()
                .HasMaxLength(CityMaxLength);

            entity
                .Property(au => au.PostalCode)
                .IsRequired()
                .HasMaxLength(PostalCodeMaxLength);

            entity
                .Property(au => au.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasData(this.GenerateSeedUser());
        }

        private ApplicationUser GenerateSeedUser()
        {
            ApplicationUser defaultUser = new ApplicationUser
            {
                Id = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                UserName = "admin@pcshop.com",
                NormalizedUserName = "ADMIN@PCSHOP.COM",
                Email = "admin@pcshop.com",
                NormalizedEmail = "ADMIN@PCSHOP.COM",
                EmailConfirmed = true,
                FullName = "Admin User",
                Address = "123 Admin Street",
                City = "Admin City",
                PostalCode = "00000",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(
                    new ApplicationUser
                    {
                        UserName = "admin@pcshop.com",
                        FullName = "Admin User",
                        Address = "123 Admin Street",
                        City = "Admin City",
                        PostalCode = "00000"
                    },
                    "Admin123!")
            };

            return defaultUser;
        }
    }
}
