using Inventory.Mostafa.Domain.Entities.CustodayTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class TransactionDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? OldRecipints { get; set; }
        public string? NewRecipints { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public string TransactionHijriDate { get; set; }
        public string? DocumentPath { get; set; }
    }
}
