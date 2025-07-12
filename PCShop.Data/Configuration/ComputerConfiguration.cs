using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.Computer;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Configuration
{
    public class ComputerConfiguration : IEntityTypeConfiguration<Computer>
    {
        public void Configure(EntityTypeBuilder<Computer> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(c => c.Price)
                .HasColumnType(PriceSqlType);

            entity
                .Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(c => c.IsDeleted == false);

            entity
                .HasData(this.GenerateSeedComputers());
        }

        private List<Computer> GenerateSeedComputers()
        {
            List<Computer> computers = new List<Computer>
            {
                new Computer
                {
                    Id = Guid.Parse("f1f11111-aaaa-bbbb-cccc-111111111111"),
                    Name = "G:RIGS POLARIS Max (AMD)",
                    Description = "CPU: AMD Ryzen 5 5600, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...",
                    Price = 2000.00m,
                    ImageUrl = "/images/computers/grigs_polaris_max_amd.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f2f22222-aaaa-bbbb-cccc-222222222222"),
                    Name = "G:RIGS POLARIS Max (Intel)",
                    Description = "CPU: Intel Core i5-13400F, GPU: GeForce RTX 3060TI, RAM: 32GB DDR4, Storage: 1TB SSD NVMe...",
                    Price = 2100.00m,
                    ImageUrl = "/images/computers/grigs_polaris_max_intel.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f3f33333-aaaa-bbbb-cccc-333333333333"),
                    Name = "G:RIGS SPARK Ultra (AMD X3D)",
                    Description = "CPU: AMD Ryzen 7 5700X3D, GPU: GeForce RTX 4060, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...",
                    Price = 2500.00m,
                    ImageUrl = "/images/computers/grigs_spark_ultra_amd_x3d.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f4f44444-aaaa-bbbb-cccc-444444444444"),
                    Name = "G:RIGS NOVA Ultra (Intel)",
                    Description = "CPU: Intel Core i5-14600KF, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...",
                    Price = 3690.00m,
                    ImageUrl = "/images/computers/grigs_nova_ultra_intel.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f5f55555-aaaa-bbbb-cccc-555555555555"),
                    Name = "G:RIGS SIRIUS Ultra (AMD Zen4)",
                    Description = "CPU: AMD Ryzen 7 7700, GPU: GeForce RTX 5070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...",
                    Price = 4200.00m,
                    ImageUrl = "/images/computers/grigs_sirius_ultra_amd_zen4_wh.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f6f66666-aaaa-bbbb-cccc-666666666666"),
                    Name = "G:RIGS SIRIUS Ultra (Intel)",
                    Description = "CPU: Intel Core i7-13700K, GPU: GeForce RTX 4070, RAM: 32GB DDR5, Storage: 2TB SSD NVMe...",
                    Price = 4500.00m,
                    ImageUrl = "/images/computers/grigs_sirius_ultra_intel.png"
                }
            };

            return computers;
        }
    }
}
