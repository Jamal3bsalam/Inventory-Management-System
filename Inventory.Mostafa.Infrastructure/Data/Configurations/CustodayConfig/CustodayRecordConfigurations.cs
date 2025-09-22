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
    public class CustodayRecordConfigurations : IEntityTypeConfiguration<CustodayRecord>
    {
        public void Configure(EntityTypeBuilder<CustodayRecord> builder)
        {
            builder.HasKey(C => C.Id);
            builder.Property(C => C.Id).UseIdentityColumn(1, 1);
        }
    }
}
