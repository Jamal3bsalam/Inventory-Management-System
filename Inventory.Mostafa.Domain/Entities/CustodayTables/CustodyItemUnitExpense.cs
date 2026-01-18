using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;

namespace Inventory.Mostafa.Domain.Entities.CustodayTables
{
    public class CustodyItemUnitExpense:BaseEntity<int>
    {
        public int? CustodyItemId { get; set; }
        public CustodyItem? CustodyItem { get; set; }

        public int? UnitExpenseItemId { get; set; }
        public UnitExpenseItems? UnitExpenseItem { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
