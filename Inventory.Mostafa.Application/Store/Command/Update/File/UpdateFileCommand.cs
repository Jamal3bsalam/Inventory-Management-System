using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Update.File
{
    public class UpdateFileCommand:IRequest<Result<string>>
    {
        public int? Id { get; set; }
        public IFormFile? File { get; set; }
    }
}
