using Inventory.Mostafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Units
{
    public class RecipientsConfiguration : IEntityTypeConfiguration<Recipients>
    {
        public void Configure(EntityTypeBuilder<Recipients> builder)
        {
            builder.HasKey(R => R.Id);
            builder.Property(R => R.Id).UseIdentityColumn(1, 1);

            builder.HasOne(R => R.Unit)
                   .WithMany(U => U.Recipients)
                   .HasForeignKey(R => R.UnitId)
                   .OnDelete(DeleteBehavior.Cascade);
                   
        }
    }
}
