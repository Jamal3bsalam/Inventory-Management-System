namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrdersForRecipintDto
    {
        public int OrderId { get; set; }
        public string SupplierName { get; set; }
        public string RecipintName { get; set; }
        public string OrderNumber { get; set; }
        public string OrderType { get; set; }
        public string OrderDate { get; set; }
        public string DocumentUrl { get; set; }
        public List<OrderItemForRecipintDto> Items { get; set; }
    }
}
