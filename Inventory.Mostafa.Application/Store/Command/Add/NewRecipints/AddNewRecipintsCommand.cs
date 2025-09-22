using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Add.NewRecipints
{
    public class AddNewRecipintsCommand:IRequest<Result<string>>
    {
        public int? UnitId { get; set; }
        public string? RecipintsName { get; set; }
    }
}
