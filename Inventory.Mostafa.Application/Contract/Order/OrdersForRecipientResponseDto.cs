namespace Inventory.Mostafa.Application.Contract.Order
{
    public class OrdersForRecipientResponseDto
    {
        public OrderTypesCountDto OrderTypesCount { get; set; }
        public List<OrdersForRecipintDto> Orders { get; set; }
    }
}
