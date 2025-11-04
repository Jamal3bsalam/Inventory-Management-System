using Inventory.Mostafa.Domain.Entities.UnitEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitExp
{
    public class UnitExpenseItemSpec:Specifications<UnitExpenseItems,int>
    {
        public UnitExpenseItemSpec(int expenseId) : base(i => i.UnitExpenseId == expenseId)
        {
            ApplyInclude();
        }

        public UnitExpenseItemSpec(int id, bool flag) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(s => s.UnitExpense);
            Include.Add(O => O.Item);
        }
    }
}
