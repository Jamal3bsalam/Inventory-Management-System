using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrdersDto
    {
        public int? Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderType { get; set; }
        public string? Attachment { get; set; }

    }
}
