using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Query.AllOpeningStock
{
    public class AllOpeningQuery:IRequest<Result<Pagination<IEnumerable<OpeningDtos>>>>
    {
        public int? pageSize { get; set; } = 10;
        public int? pageIndex { get; set; } = 1;
    }
}
