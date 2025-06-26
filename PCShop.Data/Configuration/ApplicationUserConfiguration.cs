using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.ApplicationUser;

namespace PCShop.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity
                .Property(au => au.FirstName)
                .IsRequired()
                .HasMaxLength(FirstNameMaxLength);

            entity
                .Property(au => au.LastName)
                .IsRequired()
                .HasMaxLength(LastNameMaxLength);

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
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
