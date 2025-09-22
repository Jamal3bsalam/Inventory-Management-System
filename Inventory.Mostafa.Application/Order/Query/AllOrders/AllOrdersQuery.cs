using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Query.AllOrders
{
    public class AllOrdersQuery : IRequest<Result<Pagination<IEnumerable<OrderDto>>>>
    {
        public int? pageSize { get; set; } = 10;
        public int? pageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
