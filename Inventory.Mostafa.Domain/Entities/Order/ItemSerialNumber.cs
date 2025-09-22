using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Order
{
    public class ItemSerialNumber : BaseEntity<int>
    {
        public string? SerialNumber { get; set; }
        public int? OrderItemsId { get; set; }
        public bool? IsReleased { get; set; } = false;
        public OrderItems? OrderItems { get; set; }

    }
}
