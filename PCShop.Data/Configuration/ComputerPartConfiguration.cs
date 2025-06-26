using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;

namespace PCShop.Data.Configuration
{
    public class ComputerPartConfiguration : IEntityTypeConfiguration<ComputerPart>
    {
        public void Configure(EntityTypeBuilder<ComputerPart> entity)
        {
            entity
                .HasKey(cp => new { cp.ComputerId, cp.ProductId });

            entity
                .HasOne(cp => cp.Computer)
                .WithMany(c => c.ComputersParts)
                .HasForeignKey(cp => cp.ComputerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(cp => cp.Product)
                .WithMany(p => p.ComputersParts)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(cp => cp.Computer.IsDeleted == false);
        }
    }
}
