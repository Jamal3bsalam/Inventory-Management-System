using Inventory.Mostafa.Domain.Entities.Opening;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Opening
{
    public class OpeningSerialNumberConfigurations : IEntityTypeConfiguration<OpeningSerialNumber>
    {
        public void Configure(EntityTypeBuilder<OpeningSerialNumber> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1, 1);

            builder.HasOne(S => S.OpeningStock)
                   .WithMany(O => O.SerialNumbers)
                   .HasForeignKey(S => S.OpeningStockId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
