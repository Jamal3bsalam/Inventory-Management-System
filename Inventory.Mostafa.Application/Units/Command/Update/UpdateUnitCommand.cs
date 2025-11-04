using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Command.Update
{
    public class UpdateUnitCommand:IRequest<Result<UnitDto>>
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public ICollection<UpdateRecipintsDto>? Recipients { get; set; }
    }
}
