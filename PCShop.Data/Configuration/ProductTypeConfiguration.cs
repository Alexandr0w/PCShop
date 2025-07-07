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

            entity
                .HasData(this.GenerateSeedProductTypes());
        }

        private List<ProductType> GenerateSeedProductTypes()
        {
            List<ProductType> productTypes = new List<ProductType>
            {
                new() { Id = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"), Name = "Processor" },
                new() { Id = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222"), Name = "Video Card" },
                new() { Id = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333"), Name = "Motherboard" },
                new() { Id = Guid.Parse("44444444-aaaa-bbbb-cccc-444444444444"), Name = "RAM" },
                new() { Id = Guid.Parse("55555555-aaaa-bbbb-cccc-555555555555"), Name = "SSD" },
                new() { Id = Guid.Parse("66666666-aaaa-bbbb-cccc-666666666666"), Name = "HDD" },
                new() { Id = Guid.Parse("77777777-aaaa-bbbb-cccc-777777777777"), Name = "Power Supply" },
                new() { Id = Guid.Parse("88888888-aaaa-bbbb-cccc-888888888888"), Name = "Cooling System" },
                new() { Id = Guid.Parse("99999999-aaaa-bbbb-cccc-999999999999"), Name = "Case Fan" },
                new() { Id = Guid.Parse("aaaaaaa1-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Case" },
                new() { Id = Guid.Parse("aaaaaaa2-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Monitor" },
                new() { Id = Guid.Parse("aaaaaaa3-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Keyboard" },
                new() { Id = Guid.Parse("aaaaaaa4-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Mouse" },
                new() { Id = Guid.Parse("aaaaaaa5-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Headset" },
                new() { Id = Guid.Parse("aaaaaaa6-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Mousepad" },
                new() { Id = Guid.Parse("aaaaaaa7-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Speakers" },
                new() { Id = Guid.Parse("aaaaaaa8-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Microphone" },
                new() { Id = Guid.Parse("aaaaaaa9-aaaa-bbbb-cccc-aaaaaaaaaaaa"), Name = "Webcam" }
            };
            
            return productTypes;
        }
    }
}
