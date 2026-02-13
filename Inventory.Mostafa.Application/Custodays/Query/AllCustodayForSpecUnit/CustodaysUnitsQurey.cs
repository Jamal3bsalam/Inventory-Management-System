using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Query.AllCustodayForSpecUnit
{
    public class CustodaysUnitsQurey:IRequest<Result<IEnumerable<CustodaysUnitsDto>>>
    {
        public int? UnitId { get; set; }
        public int? RecipintId { get; set; }
    }
}
