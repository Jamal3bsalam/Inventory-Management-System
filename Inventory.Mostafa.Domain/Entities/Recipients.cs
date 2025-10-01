using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
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
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<StoreRelease>? StoreReleases { get; set; }
        public int? CustodayId { get; set; }
        public Custoday? Custoday { get; set; }
        public ICollection<UnitExpense>? UnitExpenses { get; set; }
        public ICollection<Returns>? Returns { get; set; }
        public ICollection<WriteOff>? WriteOffs { get; set; }
        public ICollection<CustodayTransfers> OldTransfers { get; set; }
        public ICollection<CustodayTransfers> NewTransfers { get; set; }


    }
}
