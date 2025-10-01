using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.WriteOffConfig
{
    public class WriteOffConfiguration : IEntityTypeConfiguration<WriteOff>
    {
        public void Configure(EntityTypeBuilder<WriteOff> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).UseIdentityColumn(1, 1);

            builder.HasOne(w => w.Returns)
                   .WithMany(r => r.WriteOffs)
                   .HasForeignKey(r => r.ReturnId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(w => w.Recipients)
                   .WithMany(r => r.WriteOffs)
                   .HasForeignKey(r => r.RecipintsId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(w => w.Unit)
                   .WithMany(r => r.WriteOffs)
                   .HasForeignKey(r => r.UnitId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
