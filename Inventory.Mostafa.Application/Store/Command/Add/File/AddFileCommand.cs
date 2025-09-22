using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Add.File
{
    public class AddFileCommand : IRequest<Result<string>>
    {
        public IFormFile? File { get; set; }
    }
}
