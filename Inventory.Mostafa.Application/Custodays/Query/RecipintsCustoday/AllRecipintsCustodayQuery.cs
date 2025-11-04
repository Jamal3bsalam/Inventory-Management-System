using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Query.RecipintsCustoday
{
    public class AllRecipintsCustodayQuery:IRequest<Result<IEnumerable<RecipintsCustodayDto>>>
    {
        public int UnitId { get; set; }
        public int RecipintsId { get; set; }
    }
}
