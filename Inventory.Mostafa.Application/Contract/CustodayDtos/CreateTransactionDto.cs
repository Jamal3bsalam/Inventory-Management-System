using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class CreateTransactionDto
    {
        public int? CustodayId { get; set; }
        public int? UnitId { get; set; }
        public int? ItemId { get; set; }
        public int? NewRecipints { get; set; }
        public int? Quantity { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
