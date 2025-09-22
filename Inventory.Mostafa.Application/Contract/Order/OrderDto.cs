using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrderDto
    {
        public int? Id { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderType { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Attachment { get; set; }
        public ICollection<OrderItemDto>? Items { get; set; } = new List<OrderItemDto>();
    }
}
