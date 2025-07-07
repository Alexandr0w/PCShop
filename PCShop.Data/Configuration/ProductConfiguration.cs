using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.Product;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(p => p.Price)
                .HasColumnType(PriceSqlType);

            entity
                .Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(p => p.IsDeleted == false);

            entity
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasData(this.GenerateSeedProducts());
        }

        private List<Product> GenerateSeedProducts()
        {
            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.Parse("10101010-aaaa-bbbb-cccc-101010101010"),
                    Name = "Intel Core i7-13700K",
                    Description = "13th Gen 16-core processor with high performance for gaming and productivity.",
                    Price = 489.99m,
                    ImageUrl = "/images/products/intel-i7-13700k.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111")
                },
                new Product
                {
                    Id = Guid.Parse("20202020-aaaa-bbbb-cccc-202020202020"),
                    Name = "NVIDIA GeForce RTX 4070",
                    Description = "Latest generation graphics card with ray tracing and DLSS support.",
                    Price = 629.00m,
                    ImageUrl = "/images/products/rtx-4070.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222")
                },
                new Product
                {
                    Id = Guid.Parse("30303030-aaaa-bbbb-cccc-303030303030"),
                    Name = "Corsair Vengeance RGB Pro 32GB DDR4",
                    Description = "High-performance DDR4 memory kit with RGB lighting.",
                    Price = 139.99m,
                    ImageUrl = "/images/products/corsair-vengeance-32gb.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("44444444-aaaa-bbbb-cccc-444444444444")
                },
                new Product
                {
                    Id = Guid.Parse("40404040-aaaa-bbbb-cccc-404040404040"),
                    Name = "Samsung 980 PRO 1TB NVMe SSD",
                    Description = "Ultra-fast PCIe 4.0 SSD ideal for gaming and heavy applications.",
                    Price = 119.49m,
                    ImageUrl = "/images/products/samsung-980-pro.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("55555555-aaaa-bbbb-cccc-555555555555")
                },
                new Product
                {
                    Id = Guid.Parse("50505050-aaaa-bbbb-cccc-505050505050"),
                    Name = "ASUS ROG STRIX Z790-E",
                    Description = "Premium Z790 chipset motherboard supporting 13th Gen Intel CPUs.",
                    Price = 379.00m,
                    ImageUrl = "/images/products/asus-z790.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333")
                },
                new Product
                {
                    Id = Guid.Parse("60606060-aaaa-bbbb-cccc-606060606060"),
                    Name = "Cooler Master 750W 80+ Gold PSU",
                    Description = "Reliable and efficient 750W power supply unit with modular cables.",
                    Price = 109.99m,
                    ImageUrl = "/images/products/cooler-master-750w.jpg",
                    IsDeleted = false,
                    ProductTypeId = Guid.Parse("77777777-aaaa-bbbb-cccc-777777777777")
                }
            };

            return products;
        }
    }
}
