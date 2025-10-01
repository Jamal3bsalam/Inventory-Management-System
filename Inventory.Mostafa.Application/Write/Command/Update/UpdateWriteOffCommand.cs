using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Command.Update
{
    public class UpdateWriteOffCommand:IRequest<Result<WriteOffDto>>
    {
        public int? Id { get; set; }
        public int? Quantity { get; set; }
        public IFormFile? File { get; set; }
    }
}
