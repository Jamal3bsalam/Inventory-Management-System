using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class CustodaysUnitsDto
    {
        public int? CustodayId { get; set; }
        public int? CurrentRecipintsId { get; set; }
        public string? CurrentRecipints { get; set; }
        public ICollection<CustodaysUnitsItemsDto>? CustodaysUnitsItems { get; set; }
    }
}
