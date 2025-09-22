using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Units
{
    public class AddUnitDto
    {
        public string? UnitName { get; set; }
        public ICollection<AddRecipintsDto>? Recipients { get; set; }
    }
}
