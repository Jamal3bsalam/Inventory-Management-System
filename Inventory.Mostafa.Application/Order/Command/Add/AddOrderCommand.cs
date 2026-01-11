using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Add
{
    public class AddOrderCommand : IRequest<Result<OrderDto>>
    {
        public string? OrderNumber { get; set; }
        public OrderType? OrderType { get; set; }
        public string? SupplierName { get; set; }
        public string? RecipintName { get; set; }
        public DateOnly OrderDate { get; set; }
        public string? Attachment { get; set; }
        public List<CreateOrderItem>? Items { get; set; } = new();

    }
}
