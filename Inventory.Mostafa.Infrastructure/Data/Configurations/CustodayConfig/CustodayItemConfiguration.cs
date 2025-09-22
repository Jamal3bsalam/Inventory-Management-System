using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.CustodayConfig
{
    public class CustodayItemConfiguration : IEntityTypeConfiguration<CustodyItem>
    {
        public void Configure(EntityTypeBuilder<CustodyItem> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C => C.Id).UseIdentityColumn(1, 1);

            builder.HasOne(C => C.Item)
                   .WithMany(I => I.CustodyItems)
                   .HasForeignKey(C => C.ItemId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(C => C.Custody)
                  .WithMany(C => C.CustodyItems)
                  .HasForeignKey(C => C.CustodyId)
                  .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
