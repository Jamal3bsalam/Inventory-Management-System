using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Command.Add
{
    public class AddUnitExpenseCommand:IRequest<Result<UnitExpensDto>>
    {
        public int? UnitId { get; set; }
        public int? RecipientsId { get; set; }
        public string? ExpenseType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? DocumentNumber { get; set; }
        public DateOnly? ExpenseDate { get; set; }
        public ICollection<CreateUnitExpenseItemsDto>? UnitExpenseItemsDtos { get; set; }
    }
}
