using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Update
{
    public class DeleteOpeningCommand :IRequest<Result<string>>
    {
        public int? Id { get; set; }
    }
}
