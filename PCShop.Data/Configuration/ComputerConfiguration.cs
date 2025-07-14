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
                    Description = "CPU: AMD Ryzen 5 5600 // GPU: MSI GeForce RTX 3060 VENTUS 2X 12G // RAM: 32GB (2x 16GB) DDR4 3600 MT/s // Storage: 1TB Kingston NV3 // Motherboard: ASRock B550M Pro4 // Case: DeepCool CH360 DIGITAL // Cooling: DeepCool AG400 Black ARGB // Power Supply: DeepCool PK650D 650W Bronze",
                    Price = 1022.11m,
                    ImageUrl = "/images/computers/grigs_polaris_max_amd.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f2f22222-aaaa-bbbb-cccc-222222222222"),
                    Name = "G:RIGS POLARIS Max (Intel)",
                    Description = "CPU: Intel Core i5-13400F // GPU: MSI GeForce RTX 3060 VENTUS 2X 12G // RAM: 32GB (2x 16GB) DDR4 3600 MT/s // Storage: 1TB Kingston NV3 // Motherboard: ASUS TUF GAMING B760M-PLUS D4 // Case: DeepCool CH360 DIGITAL WH // Cooling: DeepCool AG400 White ARGB // Power Supply: DeepCool PK650D 650W Bronze",
                    Price = 1109.04m,
                    ImageUrl = "/images/computers/grigs_polaris_max_intel.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f3f33333-aaaa-bbbb-cccc-333333333333"),
                    Name = "G:RIGS SPARK Ultra (AMD X3D)",
                    Description = "CPU: AMD Ryzen 7 5700X3D // GPU: MSI GeForce RTX 4060 VENTUS 2X BLACK // RAM: 32GB (2x16GB) DDR4 3200 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B550-A PRO // Case: COUGAR Duoface RGB // Cooling: DeepCool AG400 Black ARGB // Power Supply: DeepCool PK750D 750W Bronze",
                    Price = 1241.98m,
                    ImageUrl = "/images/computers/grigs_spark_ultra_amd_x3d.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f4f44444-aaaa-bbbb-cccc-444444444444"),
                    Name = "G:RIGS NOVA Ultra (Intel)",
                    Description = "CPU: Intel Core i5-14600KF // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B760 GAMING PLUS WIFI // Case: 1stPlayer MEGAVIEW MV8 Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1",
                    Price = 1876.01m,
                    ImageUrl = "/images/computers/grigs_nova_ultra_intel.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f5f55555-aaaa-bbbb-cccc-555555555555"),
                    Name = "G:RIGS SIRIUS Ultra (AMD Zen4)",
                    Description = "CPU: AMD Ryzen 7 7700 // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B850 GAMING PLUS WIFI // Case: DeepCool CH560 DIGITAL Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1",
                    Price = 2039.63m,
                    ImageUrl = "/images/computers/grigs_sirius_ultra_amd_zen4_wh.png"
                },
                new Computer
                {
                    Id = Guid.Parse("f6f66666-aaaa-bbbb-cccc-666666666666"),
                    Name = "G:RIGS SIRIUS Ultra (Intel)",
                    Description = "CPU: Intel Core i5-14600KF // GPU: MSI GeForce RTX 5070 12G SHADOW 2X // RAM: 32GB (2x16GB) DDR5 6000 MT/s // Storage: 2TB Kingston NV3 // Motherboard: MSI B760 GAMING PLUS WIFI // Case: DeepCool CH560 DIGITAL Black // Cooling: DeepCool AG620 BK ARGB // Power Supply: DeepCool PN750M ATX 3.1",
                    Price = 1870.89m,
                    ImageUrl = "/images/computers/grigs_sirius_ultra_intel.png"
                }
            };

            return computers;
        }
    }
}
