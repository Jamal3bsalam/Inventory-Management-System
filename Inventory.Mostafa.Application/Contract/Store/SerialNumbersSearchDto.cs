using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Store
{
    public class SerialNumbersSearchDto
    {
        public string? UnitName { get; set; }
        public string? ItemName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? OrderNumber { get; set; }
        public string? SupplierName { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? SerialNumber { get; set; }
    }
}
