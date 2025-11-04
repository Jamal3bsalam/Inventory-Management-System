using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Store
{
    public class StoreReleaseItemResponseDto
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public int? OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderType { get; set; }
        public int? OrderItemId { get; set; }
        public int? Quantity { get; set; }
        public List<string>? SerialNumbers { get; set; }
    }
}
