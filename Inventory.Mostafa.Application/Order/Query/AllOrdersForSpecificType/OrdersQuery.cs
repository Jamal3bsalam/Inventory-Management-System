using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Query.AllOrdersForSpecificType
{
    public class OrdersQuery:IRequest<Result<IEnumerable<OrderDto>>>
    {
        public OrderType? OrderType { get; set; }
    }
}
