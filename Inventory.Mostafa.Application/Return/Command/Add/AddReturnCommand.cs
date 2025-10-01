using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Return.Command.Add
{
    public class AddReturnCommand:IRequest<Result<ReturnDto>>
    {
        public int? UnitId { get; set; }
        public int? RecipintsId { get; set; }
        public IFormFile? Document { get; set; }
        public int? StoreReleaseItemId { get; set; }
        public int? Quantity { get; set; }
        public string? Reason { get; set; }
    }
}
