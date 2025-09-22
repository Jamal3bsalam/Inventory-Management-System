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
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.HasKey(T => T.Id);
            builder.Property(T => T.Id).UseIdentityColumn(1,1);

            builder.HasOne(T => T.Items)
                   .WithMany(I => I.StockTransactions)
                   .HasForeignKey(T => T.ItemId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(T => T.OrderItems)
                   .WithMany(I => I.StockTransactions)
                   .HasForeignKey(T => T.OrderItemsId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
