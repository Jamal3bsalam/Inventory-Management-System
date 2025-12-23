using Inventory.Mostafa.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class UpdateOrderDto
    {
        public int? Id { get; set; }
        public string? OrderType { get; set; }
        public string? OrderNumber { get; set; }
        public string? SupplierName { get; set; }
        public string? File { get; set; }
        public List<CreateOrderItem>? Items { get; set; }

    }
}
