using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PCShop.Data.Models;
using static PCShop.Data.Common.EntityConstants.Notification;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Data.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            entity
                .HasKey(n => n.Id);

            entity.
                Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(MessageMaxLength);

            entity
                .Property(n => n.CreatedOn)
                .HasDefaultValueSql(DefaultSqlValue);

            entity
                .Property(n => n.IsRead)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(o => o.ApplicationUser.IsDeleted == false);

            entity
                .HasOne(n => n.ApplicationUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
