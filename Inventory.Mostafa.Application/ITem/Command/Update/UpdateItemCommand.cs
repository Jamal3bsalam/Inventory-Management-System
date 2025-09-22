using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Command.Update
{
    public class UpdateItemCommand:IRequest<Result<ItemDto>>
    {
        public int? Id { get; set; }
        public string? ItemsName { get; set; }
        public int? StockNumber { get; set; }
    }
}
