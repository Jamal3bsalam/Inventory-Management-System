using Inventory.Mostafa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Units
{
    public class UnitDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public ICollection<RecipintsDto>? Recipients { get; set; }
    }
}
