using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.AssetsReturns
{
    public class Returns:BaseEntity<int>
    {
        public int? UnitId { get; set; }
        public int? RecipientsId { get; set; }
        public string? DocumentPath { get; set; }
        public int? storeReleaseItemId { get; set; }
        public int? Quantity { get; set; }
        public string? Reason { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public StoreReleaseItem? StoreReleaseItem { get; set; }
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }

        public ICollection<WriteOff>? WriteOffs { get; set; }
    }
}
