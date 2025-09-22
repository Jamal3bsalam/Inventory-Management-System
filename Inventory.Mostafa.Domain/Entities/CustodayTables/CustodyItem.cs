using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.CustodayTables
{
    public class CustodyItem:BaseEntity<int>
    {
        public int? CustodyId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }

        // Navigation
        public Custoday? Custody { get; set; }
        public Items? Item { get; set; }
    }
}
