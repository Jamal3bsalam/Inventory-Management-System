using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.UnitEx
{
    public class UnitExpenseItems:BaseEntity<int>
    {
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? UnitExpenseId { get; set; }
        public int? Quantity { get; set; }
        public Items? Item { get; set; }
        public UnitExpense? UnitExpense { get; set; }
        public ICollection<CustodayTransfers> CustodayTransfers { get; set; } = new List<CustodayTransfers>();
        public ICollection<CustodyItemUnitExpense>? CustodyLinks { get; set; }


    }
}
