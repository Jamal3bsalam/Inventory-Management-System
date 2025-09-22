using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Store
{
    public class ReleaseItemSerialNumber:BaseEntity<int>
    {
        public int StoreReleaseItemId { get; set; }
        public string? SerialNumber { get; set; }

        public StoreReleaseItem? StoreReleaseItem { get; set; }
    }
}
