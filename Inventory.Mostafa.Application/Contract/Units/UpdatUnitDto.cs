using Inventory.Mostafa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Units
{
    public class UpdatUnitDto
    {
        public string? UnitName { get; set; }
        public ICollection<UpdateRecipintsDto>? Recipients { get; set; }

    }
}
