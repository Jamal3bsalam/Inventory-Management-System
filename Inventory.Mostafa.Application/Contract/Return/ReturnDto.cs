using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Return
{
    public class ReturnDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? RecipintsName { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
        public string? Reason { get; set; }
        public string? DocumentUrl { get; set; }
    }
}
