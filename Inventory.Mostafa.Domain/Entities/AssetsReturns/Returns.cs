using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
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
        public int? ExpenseId { get; set; }
        public int? Quantity { get; set; }
        public int WriteOfQuantity { get; set; } = 0;   
        public string? Reason { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public UnitExpense? Expense { get; set; }
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }
        public int? ItemId { get; set; }
        public Items? Item { get; set; }
        public ICollection<WriteOff>? WriteOffs { get; set; }
    }
}
