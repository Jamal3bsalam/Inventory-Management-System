using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class CreateOrderItem
    {
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public ICollection<string>? SerialNumbers { get; set; }
    }
}
