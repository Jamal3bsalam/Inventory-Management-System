using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.CustodayDtos
{
    public class CustodayItemDto
    {
        public int CustodayId { get; set; }
        public int CurrentRecipints { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
