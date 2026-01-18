using Inventory.Mostafa.Domain.Entities.CustodayTables;

namespace Inventory.Mostafa.Domain.Specification.CustodaySpecificaion
{
    public class CustodyItemUnitExpenseSpec:Specifications<CustodyItemUnitExpense,int>
    {
        public CustodyItemUnitExpenseSpec(List<int> expenseItemIds):base(C => expenseItemIds.Contains((int)C.UnitExpenseItemId))
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(C => C.CustodyItem);
            Include.Add(C => C.CustodyItem.Custody.Recipients);
            Include.Add(C => C.UnitExpenseItem);
        }
    }
}
