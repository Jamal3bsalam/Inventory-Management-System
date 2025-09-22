using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
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
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Recipients>? Recipients { get; set; } = new List<Recipients>();
        public ICollection<StoreRelease>? StoreReleases { get; set; }
        public ICollection<Custoday>? Custodays { get; set; }
        public ICollection<UnitExpense>? UnitExpenses { get; set; }

    }
}
