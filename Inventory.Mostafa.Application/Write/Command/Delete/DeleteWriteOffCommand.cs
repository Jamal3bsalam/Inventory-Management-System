using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Command.Delete
{
    public class DeleteWriteOffCommand:IRequest<Result<string>>
    {
        public int? Id { get; set; }
    }
}
