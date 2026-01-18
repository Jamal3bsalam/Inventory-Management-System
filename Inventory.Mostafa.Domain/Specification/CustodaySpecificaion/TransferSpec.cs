using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Specification.Store;
namespace Inventory.Mostafa.Domain.Specification.CustodaySpecificaion
{
    public class TransferSpec:Specifications<CustodayTransfers,int>
    {
        public TransferSpec(StoreReleaseSpecParameter parameter) 
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public TransferSpec(StoreReleaseSpecParameter parameter,bool flag):base(t => t.UnitId ==  parameter.UnitId)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public TransferSpec(int? unitId, List<int?> expenseItemIds): base(t => (!unitId.HasValue || t.UnitId == unitId) && expenseItemIds.Contains(t.UnitExpenseItemsId))
        {
            ApplyInclude();
        }


        private void ApplyInclude()
        {
            Include.Add(C => C.Item);
            Include.Add(C => C.OldRecipient);
            Include.Add(C => C.NewRecipient);
            Include.Add(C => C.Unit);

        }
    }
}
