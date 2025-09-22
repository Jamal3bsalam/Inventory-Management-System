using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Command.Delete
{
    public class DeleteUnitExpenseCommand:IRequest<Result<string>>
    {
        public int? Id { get; set; }
    }
}
