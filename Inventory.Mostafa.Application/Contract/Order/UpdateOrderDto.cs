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
        public OrderType? OrderType { get; set; }
        public int? OrderNumber { get; set; }
        public IFormFile? File { get; set; }
    }
}
