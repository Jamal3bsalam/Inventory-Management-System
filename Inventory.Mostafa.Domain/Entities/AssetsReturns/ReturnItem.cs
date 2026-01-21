using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;

namespace Inventory.Mostafa.Domain.Entities.AssetsReturns
{
    public class ReturnItem:BaseEntity<int>
    {
        public int ReturnId { get; set; }
        public int UnitExpenseId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        //Navigation Properties
        public UnitExpense? UnitExpense { get; set; }
        public Items? Item { get; set; }
        public Returns? Return { get; set; }
    }
}
