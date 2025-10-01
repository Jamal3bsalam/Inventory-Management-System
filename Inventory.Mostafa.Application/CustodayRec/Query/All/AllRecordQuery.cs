using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.CustodayRec.Query.All
{
    public class AllRecordQuery : IRequest<Result<IEnumerable<RecordDto>>>
    {
    }
}
