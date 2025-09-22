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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(oi => oi.Id).UseIdentityColumn(1, 1);

            builder.HasOne(oi => oi.Orders)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrdersId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Items)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.ItemsId)
                  .OnDelete(DeleteBehavior.NoAction);




        }
    }
}
