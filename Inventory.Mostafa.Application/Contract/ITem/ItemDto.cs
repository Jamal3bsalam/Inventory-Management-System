using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.ITem
{
    public class ItemDto
    {
        public int? Id { get; set; }
        public string? ItemsName { get; set; }
        public int? StockNumber { get; set; }
    }
}
