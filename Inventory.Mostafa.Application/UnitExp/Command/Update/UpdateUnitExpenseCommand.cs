using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Command.Update
{
    public class UpdateUnitExpenseCommand:IRequest<Result<UnitExpensDto>>
    {
        public int? UnitExpenseId { get; set; }
        public int? UnitId { get; set; }
        public int? RecipientsId { get; set; }
        public string? ExpenseType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public ICollection<CreateUnitExpenseItemsDto>? UnitExpenseItemsDtos { get; set; }
    }
}
