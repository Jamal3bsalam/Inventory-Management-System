using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Return.Query.AllReturns
{
    public class ReturnsQuery:IRequest<Result<Pagination<IEnumerable<AllReturnDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}
