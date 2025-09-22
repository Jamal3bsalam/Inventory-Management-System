using Inventory.Mostafa.Domain.Entities.UnitEx;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.UnitExp
{
    public class UnitExpenseConfiguration : IEntityTypeConfiguration<UnitExpense>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UnitExpense> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);

            builder.HasIndex(sr => sr.DocumentNumber)
               .IsUnique()
               .HasFilter("[DocumentNumber] IS NOT NULL");

            builder.HasOne(U => U.Unit)
                   .WithMany(U => U.UnitExpenses)
                   .HasForeignKey(U => U.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(U => U.Recipients)
                   .WithMany(R => R.UnitExpenses)
                   .HasForeignKey(U => U.RecipientsId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
