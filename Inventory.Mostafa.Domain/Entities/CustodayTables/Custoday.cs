using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.CustodayTables
{
    public class Custoday:BaseEntity<int>
    {
        public int UnitId { get; set; }       // الوحدة المرتبطة بالعهدة
        public int RecipientsId { get; set; }   // الشخص اللي ماسك العهدة
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateOnly? TransactionDate { get; set; }
        public string? DocumentPath { get; set; }

        // Navigation
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }
        public ICollection<CustodyItem>? CustodyItems { get; set; }
    }
}
