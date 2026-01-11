namespace Inventory.Mostafa.Application.Contract.UnitExp
{
    public class UnitExpenseDetailsDto
    {
        public int? Id { get; set; }
        public string? UnitName { get; set; }
        public string? RecipientsName { get; set; }
        public string? ExpenseType { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? DocumentNumber { get; set; }
        public DateOnly? ExpenseDate { get; set; }
        public List<string>? OldRecipintsRecipients { get; set; }
        public List<string>? CustodaysFiles { get; set; }
        public ICollection<UnitExpensItemsDto>? UnitExpenseItemsDtos { get; set; }
    }
}
