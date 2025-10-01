using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.CustodayTables
{
    public class CustodayTransfers:BaseEntity<int>
    {
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }

        public int ItemId { get; set; }
        public Items? Item { get; set; }

        public int OldRecipientId { get; set; }
        public Recipients? OldRecipient { get; set; }

        public int NewRecipientId { get; set; }
        public Recipients? NewRecipient { get; set; }

        public int Quantity { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string? DocumentPath { get; set; }
    }
}
