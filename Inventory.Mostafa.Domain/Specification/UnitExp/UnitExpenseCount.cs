using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitExp
{
    public class UnitExpenseCount : Specifications<UnitExpense,int>
    {
        public UnitExpenseCount(StoreReleaseSpecParameter specParameter) : base(U => U.IsDeleted != true)
        {
        }

        public UnitExpenseCount(StoreReleaseSpecParameter specParameter, bool flag) : base(U => U.IsDeleted != true && U.UnitId == specParameter.UnitId)
        {
        }
    }
}
