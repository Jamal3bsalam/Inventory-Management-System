namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrderItemForRecipintDto
    {
        public string? ItemName { get; set; }
        public int? TotalQuantity { get; set; }
        public int? ConsumedQuantity { get; set; }
        public int? RemainingQuantity => TotalQuantity - ConsumedQuantity;
    }
}
