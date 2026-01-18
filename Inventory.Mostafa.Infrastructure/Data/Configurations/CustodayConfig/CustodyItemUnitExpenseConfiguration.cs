using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.CustodayConfig
{
    public class CustodyItemUnitExpenseConfiguration : IEntityTypeConfiguration<CustodyItemUnitExpense>
    {
        public void Configure(EntityTypeBuilder<CustodyItemUnitExpense> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C => C.Id).UseIdentityColumn(1, 1);

            builder.HasOne(x => x.CustodyItem)
               .WithMany(c => c.UnitExpenseLinks)
               .HasForeignKey(x => x.CustodyItemId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UnitExpenseItem)
                   .WithMany(e => e.CustodyLinks)
                   .HasForeignKey(x => x.UnitExpenseItemId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
