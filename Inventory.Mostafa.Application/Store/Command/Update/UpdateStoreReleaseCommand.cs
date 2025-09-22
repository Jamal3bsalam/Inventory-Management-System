using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Update
{
    public class UpdateStoreReleaseCommand:IRequest<Result<CreateStoreReleaseDto>>
    {
       
    }
}
