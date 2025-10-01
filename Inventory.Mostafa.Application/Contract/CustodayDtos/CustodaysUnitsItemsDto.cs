using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class CustodaysUnitsItemsDto
    {
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }

    }
}
