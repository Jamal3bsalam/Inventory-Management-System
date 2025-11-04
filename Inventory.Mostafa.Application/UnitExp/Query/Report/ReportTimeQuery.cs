using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Query.Report
{
    public class ReportTimeQuery:IRequest<Result<IEnumerable<UnitExpensDto>>>
    {
        public int? UnitId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
