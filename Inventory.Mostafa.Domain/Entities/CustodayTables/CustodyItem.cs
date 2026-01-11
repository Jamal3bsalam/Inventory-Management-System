using Inventory.Mostafa.Domain.Shared;
namespace Inventory.Mostafa.Domain.Entities.CustodayTables
{
    public class CustodyItem:BaseEntity<int>
    {
        public int? CustodyId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }

        // Navigation
        public Custoday? Custody { get; set; }
        public Items? Item { get; set; }
    }
}
