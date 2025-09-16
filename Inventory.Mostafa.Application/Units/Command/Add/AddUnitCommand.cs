using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Command.Add
{
    public class AddUnitCommand : IRequest<Result<UnitDto>>
    {
        public string? UnitName { get; set; }
        public ICollection<Recipients>? Recipients { get; set; }
    }
}
