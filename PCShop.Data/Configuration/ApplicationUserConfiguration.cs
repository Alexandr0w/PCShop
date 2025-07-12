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
                .HasQueryFilter(au => au.IsDeleted == false);

            entity
                .HasData(this.GenerateSeedUser());
        }

        private ApplicationUser GenerateSeedUser()
        {
            ApplicationUser defaultUser = new ApplicationUser
            {
                // UserName: admin@pcshop.com
                // Password: Admin123!
                Id = Guid.Parse("df1c3a0f-1234-4cde-bb55-d5f15a6aabcd"),
                UserName = "admin@pcshop.com",
                NormalizedUserName = "ADMIN@PCSHOP.COM",
                Email = "admin@pcshop.com",
                NormalizedEmail = "ADMIN@PCSHOP.COM",
                EmailConfirmed = true,
                FullName = "Admin User",
                Address = "123 Admin Street",
                City = "Admin City",
                PostalCode = "0000",
                SecurityStamp = "cdd1077d-f80c-4f64-b3bc-e7b71fc4ef9a",
                ConcurrencyStamp = "20156ffd-ca5b-493f-8cdb-f0e37d6ffc28",
                PasswordHash = "AQAAAAIAAYagAAAAEN6HKPt1nIvfpyU80C1hHBdt/MBp8t8qhTJqbc55PHUBGvA9g2Y47ybCLa5zyIrOPg=="
            };

            return defaultUser;
        }
    }
}
