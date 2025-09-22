using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitExp
{
    public class UnitExpenseSpec : Specifications<UnitExpense,int>
    {

        public UnitExpenseSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public UnitExpenseSpec(StoreReleaseSpecParameter parameter) : base(S => S.IsDeleted != true)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public UnitExpenseSpec(StoreReleaseSpecParameter parameter, bool flag) : base(S => S.IsDeleted != true && S.UnitId == parameter.UnitId)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Include.Add(S => S.Unit);
            Include.Add(O => O.Recipients);
            Include.Add(sr => sr.ExpenseItems);
            IncludeStrings.Add($"{nameof(UnitExpense.ExpenseItems)}.{nameof(UnitExpenseItems.Item)}");
        }
    }
}
