namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreReleaseItemParameters
    {
        public int? UnitId { get; set; }
        public int? ItemId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; } = 1;
    }
}
