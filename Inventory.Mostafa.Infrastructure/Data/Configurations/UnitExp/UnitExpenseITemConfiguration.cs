using Inventory.Mostafa.Domain.Entities.UnitEx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.UnitExp
{
    public class UnitExpenseITemConfiguration : IEntityTypeConfiguration<UnitExpenseItems>
    {
        public void Configure(EntityTypeBuilder<UnitExpenseItems> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);

            builder.HasOne(U => U.Item)
                   .WithMany(U => U.UnitExpenseItems)
                   .HasForeignKey(U => U.ItemId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(U => U.UnitExpense)
                   .WithMany(E => E.ExpenseItems)
                   .HasForeignKey(U => U.UnitExpenseId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
