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
        public int UnitId { get; set; }
        public int NewRecipints { get; set; }
        public string TransactionHijriDate { get; set; }
        public string? FileName { get; set; }
        public ICollection<CustodayItemDto>? Items { get; set; }

    }
}
