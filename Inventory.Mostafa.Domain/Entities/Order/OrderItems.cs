using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Order
{
    public class OrderItems:BaseEntity<int>
    {
        public int? OrdersId { get; set; }
        public Orders? Orders { get; set; }
        public string? ItemName { get; set; }
        public int? ItemsId { get; set; }
        public Items? Items { get; set; }
        public int? StockNumber { get; set; }
        public int? Quantity { get; set; }
        public int? ConsumedQuantity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<ItemSerialNumber>? SerialNumbers { get; set; } = new List<ItemSerialNumber>();
        public ICollection<StoreReleaseItem>? StoreReleaseItems { get; set; } = new List<StoreReleaseItem>();
        public ICollection<StockTransaction>? StockTransactions { get; set; } 
    }
}
