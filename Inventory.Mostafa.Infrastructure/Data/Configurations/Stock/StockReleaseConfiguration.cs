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
    public class StockReleaseConfiguration : IEntityTypeConfiguration<StoreRelease>
    {
        public void Configure(EntityTypeBuilder<StoreRelease> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);

            builder.HasOne(S => S.Unit)
                   .WithMany(U => U.StoreReleases)
                   .HasForeignKey(S => S.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(S => S.Recipients)
                   .WithMany(R => R.StoreReleases)
                   .HasForeignKey(S => S.RecipientsId)
                   .OnDelete(DeleteBehavior.NoAction);

            
        }
    }
}
