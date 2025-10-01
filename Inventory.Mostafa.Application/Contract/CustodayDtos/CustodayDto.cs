using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class CustodayDto
    {
        public string? UnitName { get; set; }
        public string? OldRecipints { get; set; }
        public string? NewRecipints { get; set; }
        public ICollection<CustodaysUnitsItemsDto>? ItemsDtos { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public string? DocumentPath { get; set; }
    }
}
