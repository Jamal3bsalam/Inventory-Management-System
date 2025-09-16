using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Configurations.Units
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(U => U.Id);
            builder.Property(U => U.Id).UseIdentityColumn(1, 1);

            
        }
    }
}
