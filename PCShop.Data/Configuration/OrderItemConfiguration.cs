using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.OrderItem;

namespace PCShop.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity
                .HasKey(oi => oi.Id);

            entity
                .Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(QuantityDefaultValue);

            entity
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrdersItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrdersItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(oi => oi.Computer)
                .WithMany(c => c.OrdersItems)
                .HasForeignKey(oi => oi.ComputerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(oi =>
                (oi.Product != null && !oi.Product.IsDeleted) ||
                (oi.Computer != null && !oi.Computer.IsDeleted));

        }
    }
}
