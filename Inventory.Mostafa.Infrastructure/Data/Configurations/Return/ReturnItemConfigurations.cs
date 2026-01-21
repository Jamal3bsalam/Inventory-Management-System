using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Return
{
    public class ReturnItemConfigurations : IEntityTypeConfiguration<ReturnItem>
    {
        public void Configure(EntityTypeBuilder<ReturnItem> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).UseIdentityColumn(1, 1);

            builder.HasOne(r => r.UnitExpense)
                   .WithMany(U => U.ReturnItems)
                   .HasForeignKey(r => r.UnitExpenseId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Item)
                   .WithMany(i => i.ReturnItems)
                   .HasForeignKey(r => r.ItemId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
