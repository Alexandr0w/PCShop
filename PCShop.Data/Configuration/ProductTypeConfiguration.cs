using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.ProductType;

namespace PCShop.Data.Configuration
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> entity)
        {
            entity
                .HasKey(pt => pt.Id);

            entity
                .Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);
        }
    }
}
