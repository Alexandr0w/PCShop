using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using static PCShop.Data.Common.EntityConstants.Order;

namespace PCShop.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasKey(o => o.Id);

            entity
                .Property(o => o.OrderDate)
                .IsRequired()
                .HasDefaultValueSql(OrderDateDefaultValue);

            entity
                .Property(o => o.Status)
                .IsRequired()
                .HasDefaultValue(OrderStatus.Pending);

            entity
                .Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType(TotalPriceType);

            entity
                .HasOne(o => o.ApplicationUser)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
