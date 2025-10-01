using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Entities.Store
{
    public class StoreRelease:BaseEntity<int>
    {
        public int UnitId { get; set; }
        public int RecipientsId { get; set; }
        public string? DocumentNumber { get; set; } = string.Empty;
        public string? DocumentPath { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation
        public Unit? Unit { get; set; }
        public Recipients? Recipients { get; set; }
        public ICollection<StoreReleaseItem>? StoreReleaseItems { get; set; }
    }
}
