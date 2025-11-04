using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Write.Command.Add
{
    public class WriteOffCommand:IRequest<Result<WriteOffDto>>
    {
        public int? UnitId { get; set; }
        public int? RecipintsId { get; set; }
        public int? ReturnsId { get; set; }
        public int Quantity { get; set; }
        public IFormFile? Documet { get; set; }
    }
}
