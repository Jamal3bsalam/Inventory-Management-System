using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Units
{
    public class RecipintsCustodayDto
    {
        public string? ItemName { get; set; }
        public int? OriginalQuantity { get; set; }
        public int? ReturnedQuantity { get; set; }
        public int? RemainingQuantity { get; set; }
    }
}
