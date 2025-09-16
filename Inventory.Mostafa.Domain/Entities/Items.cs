using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities
{
    public class Items : BaseEntity<int>
    {
        public string? ItemsName { get; set; }
        public int? StockNumber { get; set; }
        public ICollection<OpeningStock>? OpeningStocks { get; set; }
        public ICollection<OrderItems>? OrderItems { get; set; }
    }
}
