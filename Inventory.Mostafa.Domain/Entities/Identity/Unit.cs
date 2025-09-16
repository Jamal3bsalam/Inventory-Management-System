using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Identity
{
    public class Unit : BaseEntity<int>
    {
        public string? UnitName { get; set; }
        public ICollection<Recipients>? Recipients { get; set; } = new List<Recipients>();
    }
}
