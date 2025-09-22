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
    public class OpeningStockConfigurations : IEntityTypeConfiguration<OpeningStock>
    {
        public void Configure(EntityTypeBuilder<OpeningStock> builder)
        {
            builder.HasKey(O => O.Id);
            builder.Property(O => O.Id).UseIdentityColumn(1, 1);

            builder.HasOne(O => O.Items)
                   .WithMany(I => I.OpeningStocks)
                   .HasForeignKey(O => O.ItemsId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
