namespace Inventory.Mostafa.Application.Contract.Return
{
    public class ReturnItemResponseDto
    {
        public int ExpenseId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentUrl { get; set; }
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
    }
}
