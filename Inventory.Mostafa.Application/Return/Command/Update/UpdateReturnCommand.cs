using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Return.Command.Update
{
    public class UpdateReturnCommand:IRequest<Result<ReturnDto>>
    {
        public int? ReturnId { get; set; }
        public int? Quantity { get; set; }
        public IFormFile? File { get; set; }
        public string? Reason { get; set; }
    }
}
