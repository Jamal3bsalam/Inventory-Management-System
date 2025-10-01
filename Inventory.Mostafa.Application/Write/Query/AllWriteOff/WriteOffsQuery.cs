using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Query.AllWriteOff
{
    public class WriteOffsQuery:IRequest<Result<Pagination<IEnumerable<WriteOffDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}
