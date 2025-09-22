using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Store
{
    public class StockTransaction : BaseEntity<int>
    {
        public int? ItemId { get; set; }
        public int? OrderItemsId { get; set; }
        public string? TransactionType { get; set; } = "Release"; // هنا غالباً هتكون "Release"
        public int? Quantity { get; set; }  // كمية سالبة عند الصرف
        public int? BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int RelatedId { get; set; } // ممكن نخزن فيه ReleaseId
        public string? RefrenceType { get; set; }

        public Items? Items { get; set; }
        public OrderItems? OrderItems { get; set; }
    }
}
