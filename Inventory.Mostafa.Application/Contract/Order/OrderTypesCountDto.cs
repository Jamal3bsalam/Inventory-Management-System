using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrderTypesCountDto
    {
        public int EndorsementCont { get; set; }
        public int PurchaseCount { get; set; }
        public int SupportCount { get; set; }
        public int ReturnCount { get; set; }
        public int TotalCount { get; set; }
    }
}
