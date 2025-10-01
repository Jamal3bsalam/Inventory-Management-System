using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.CustodayTransferConfig
{
    public class TransferConfiguration : IEntityTypeConfiguration<CustodayTransfers>
    {
        public void Configure(EntityTypeBuilder<CustodayTransfers> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).UseIdentityColumn(1, 1);

            builder.HasOne(t => t.Unit)
                  .WithMany(u => u.CustodayTransfers)
                  .HasForeignKey(t => t.UnitId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Item)
                  .WithMany(u => u.CustodayTransfers)
                  .HasForeignKey(t => t.ItemId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.OldRecipient)
                  .WithMany(u => u.OldTransfers)
                  .HasForeignKey(t => t.OldRecipientId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.NewRecipient)
                  .WithMany(u => u.NewTransfers)
                  .HasForeignKey(t => t.NewRecipientId)
                  .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
