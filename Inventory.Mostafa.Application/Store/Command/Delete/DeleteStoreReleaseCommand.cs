using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Delete
{
    public class DeleteStoreReleaseCommand:IRequest<Result<string>>
    {
        public int? Id { get; set; }
    }
}
