using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Store
{
    public class StoreReleaseItem : BaseEntity<int>
    {
        public int StoreReleaseId { get; set; }
        public int? ItemId { get; set; }
        public int OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? OrderItemId { get; set; }
        public OrderItems? OrderItem { get; set; }
        public ICollection<ReleaseItemSerialNumber>? SerialNumbers { get; set; }
        public ICollection<Returns>? Returns { get; set; }

        // Navigation
        public StoreRelease? StoreRelease { get; set; }
        public Items? Items { get; set; }
        public Orders? Order { get; set; }
    }
}
