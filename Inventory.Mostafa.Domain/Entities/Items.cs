using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
namespace Inventory.Mostafa.Domain.Entities
{
    public class Items : BaseEntity<int>
    {
        public string? ItemsName { get; set; }
        public int? StockNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<OpeningStock>? OpeningStocks { get; set; }
        public ICollection<OrderItems>? OrderItems { get; set; }
        public ICollection<StoreReleaseItem>? StoreReleaseItems { get; set; }
        public ICollection<CustodyItem>? CustodyItems { get; set; }
        public ICollection<StockTransaction>? StockTransactions { get; set; }
        public ICollection<UnitExpenseItems>? UnitExpenseItems { get; set; }
        public ICollection<CustodayTransfers>? CustodayTransfers { get; set; }
        public ICollection<ReturnItem>? ReturnItems { get; set; }
    }
}
