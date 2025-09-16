using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Delete
{
    public class UpdateOpeningCommand:IRequest<Result<OpeningDto>>
    {
        public int? Id { get; set; }
        public int? NewQuantity { get; set; }
        public ICollection<string>? NewSerialNumbers { get; set; }
    }
}
