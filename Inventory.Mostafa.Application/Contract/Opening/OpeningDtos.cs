using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Opening
{
    public class OpeningDtos
    {
        public int? Id { get; set; }
        public string? ItemName { get; set; }
        public int? StockNumber { get; set; }
        public int? Quantity { get; set; }
    }
}
