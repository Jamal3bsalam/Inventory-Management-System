using Inventory.Mostafa.Domain.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Stock
{
    public class ReleaseItemsConfiguration : IEntityTypeConfiguration<StoreReleaseItem>
    {
        public void Configure(EntityTypeBuilder<StoreReleaseItem> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);

            builder.HasOne(S => S.Items)
                   .WithMany(I => I.StoreReleaseItems)
                   .HasForeignKey(S => S.ItemId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(S => S.Order)
                   .WithMany(O => O.StoreReleaseItems)
                   .HasForeignKey(S => S.OrderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(S => S.StoreRelease)
                   .WithMany(S => S.StoreReleaseItems)
                   .HasForeignKey(S => S.StoreReleaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(S => S.OrderItem)
                    .WithMany(O => O.StoreReleaseItems)
                    .HasForeignKey(S => S.OrderItemId)
                    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
