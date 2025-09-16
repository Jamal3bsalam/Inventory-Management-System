using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities
{
    public class Recipients : BaseEntity<int>
    {
        public string? Name { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
    }
}
