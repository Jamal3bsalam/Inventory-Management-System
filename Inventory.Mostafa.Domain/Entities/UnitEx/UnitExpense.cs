using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.UnitEx
{
    public class UnitExpense:BaseEntity<int>
    {
        public int? UnitId { get; set; }
        public int? RecipientsId { get; set; }
        public string? ExpenseType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }

        public ICollection<UnitExpenseItems>? ExpenseItems { get; set; }
    }
}
