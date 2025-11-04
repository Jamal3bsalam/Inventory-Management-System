using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Command.Delete
{
    public class DeleteRecipintsCommand:IRequest<Result<string>>
    {
        public int? UnitId { get; set; }
        public int? RecipintsId { get; set; }
    }
}
