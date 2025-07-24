using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using static PCShop.Data.Common.EntityConstants.Order;
using static PCShop.GCommon.ApplicationConstants;

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
                .HasDefaultValueSql(OrderDateDefaultSqlValue);

            entity
                .Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Pending)
                .HasConversion<int>(); // Save enum like int in DB

            entity
                .Property(o => o.TotalPrice)
                .HasColumnType(PriceSqlType);

            entity
                .Property(o => o.Comment)
                .IsRequired(false)
                .HasMaxLength(CommentMaxLength);

            entity
                .Property(o => o.DeliveryMethod)
                .IsRequired()
                .HasConversion<int>(); // Save enum like int in DB

            entity
                .Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<int>(); // Save enum like int in DB

            entity
                .Property(o => o.DeliveryFee)
                .HasColumnType(PriceSqlType);

            entity
                .Property(o => o.DeliveryAddress)
                .IsRequired(false)
                .HasMaxLength(DeliveryAddressMaxLength);

            entity
                .Property(o => o.SendDate)
                .IsRequired(false);

            entity
                .HasQueryFilter(o => o.ApplicationUser.IsDeleted == false);

            entity
                .HasOne(o => o.ApplicationUser)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
