using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.AssetsReturns
{
    public class WriteOff:BaseEntity<int>
    {
        public int? ReturnId { get; set; }
        public int? UnitId { get; set; }
        public int? RecipintsId { get; set; }
        public int? Quantity { get; set; }
        public string? DocumentPath { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }
        public Returns? Returns { get; set; }
    }
}
