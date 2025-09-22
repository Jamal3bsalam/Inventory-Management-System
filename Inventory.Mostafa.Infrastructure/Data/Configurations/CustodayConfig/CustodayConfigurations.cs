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
    public class CustodayConfigurations : IEntityTypeConfiguration<Custoday>
    {
        public void Configure(EntityTypeBuilder<Custoday> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C => C.Id).UseIdentityColumn(1, 1);

            builder.HasOne(C => C.Unit)
                   .WithMany(U => U.Custodays)
                   .HasForeignKey(C => C.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(C => C.Recipients)
                   .WithOne(R => R.Custoday)
                   .HasForeignKey<Custoday>(C => C.RecipientsId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
