using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Opening
{
    public class OpeningStock : BaseEntity<int>
    {
        public int? ItemsId { get; set; }
        public Items? Items { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public ICollection<OpeningSerialNumber>? SerialNumbers { get; set; }
    }
}
