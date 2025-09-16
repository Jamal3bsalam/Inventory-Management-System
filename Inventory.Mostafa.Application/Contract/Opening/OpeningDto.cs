using Inventory.Mostafa.Domain.Entities.Opening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Opening
{
    public class OpeningDto
    {
        public int? Id { get; set; }
        public string? ItemName { get; set; }
        public int? StockNumber { get; set; }
        public int? Quantity { get; set; }
        public ICollection<string>? SerialNumbers { get; set; }
    }
}
