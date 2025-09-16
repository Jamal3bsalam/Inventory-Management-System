using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Add
{
    public class AddOpeningCommand:IRequest<Result<OpeningDto>>
    {
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public ICollection<string>? SerialNumbers { get; set; }

    }
}
