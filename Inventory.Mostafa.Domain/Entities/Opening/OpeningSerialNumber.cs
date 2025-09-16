using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Opening
{
    public class OpeningSerialNumber:BaseEntity<int>
    {
        public string? SerialNumber { get; set; }
        public int OpeningStockId { get; set; }   // FK → OpeningStock
        public OpeningStock? OpeningStock { get; set; }
    }
}
