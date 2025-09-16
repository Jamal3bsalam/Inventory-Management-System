using Inventory.Mostafa.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Order
{
    public class SerialNumberConfigurations : IEntityTypeConfiguration<ItemSerialNumber>
    {
        public void Configure(EntityTypeBuilder<ItemSerialNumber> builder)
        {
            builder.HasKey(S => S.Id);
            builder.Property(S => S.Id).UseIdentityColumn(1,1);

            builder.HasOne(S => S.OrderItems)
                   .WithMany(O => O.SerialNumbers)
                   .HasForeignKey(S => S.OrderItemsId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
