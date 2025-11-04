using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Query.AllRecipintsForUnit
{
    public class UnitRecipintsQuery:IRequest<Result<IEnumerable<RecipintsDto>>>
    {
        public int? UnitId { get; set; }
    }
}
