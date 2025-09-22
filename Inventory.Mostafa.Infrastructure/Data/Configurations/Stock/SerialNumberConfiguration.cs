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
    public class SerialNumberConfiguration : IEntityTypeConfiguration<ReleaseItemSerialNumber>
    {
        public void Configure(EntityTypeBuilder<ReleaseItemSerialNumber> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1,1);

            builder.HasOne(S => S.StoreReleaseItem)
                   .WithMany(S => S.SerialNumbers)
                   .HasForeignKey(S => S.StoreReleaseItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
