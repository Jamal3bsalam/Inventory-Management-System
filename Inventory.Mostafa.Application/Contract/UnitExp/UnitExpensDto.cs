using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.UnitExp
{
    public class UnitExpensDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? RecipientsName { get; set; }
        public string? ExpenseType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? DocumentNumber { get; set; }
        public DateOnly? ExpenseDate { get; set; }
        public ICollection<UnitExpensItemsDto>? UnitExpenseItemsDtos { get; set; }
    }
}
