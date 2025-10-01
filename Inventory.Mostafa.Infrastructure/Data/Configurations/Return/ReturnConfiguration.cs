using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Return
{
    public class ReturnConfiguration : IEntityTypeConfiguration<Returns>
    {
        public void Configure(EntityTypeBuilder<Returns> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).UseIdentityColumn(1, 1);

            builder.HasOne(r => r.StoreReleaseItem)
                   .WithMany(s => s.Returns)
                   .HasForeignKey(r => r.storeReleaseItemId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Unit)
                   .WithMany(s => s.Returns)
                   .HasForeignKey(r => r.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Recipients)
                   .WithMany(s => s.Returns)
                   .HasForeignKey(r => r.RecipientsId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
