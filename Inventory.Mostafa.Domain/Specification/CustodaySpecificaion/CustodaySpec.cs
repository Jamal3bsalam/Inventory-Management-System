using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.CustodaySpecificaion
{
    public class CustodaySpec:Specifications<Custoday,int>
    {
        public CustodaySpec(int recipintsId) : base(C => C.RecipientsId == recipintsId)
        {
            ApplyInclude();
        }

        public CustodaySpec(int unitId,bool flag) : base(c => c.UnitId == unitId)
        {
            ApplyInclude();
        }

        public CustodaySpec(int recipintsId, bool flag1,bool flag2,bool flag3) : base(c => c.RecipientsId == recipintsId)
        {
            ApplyInclude();
        }

        public CustodaySpec(int custodyId,bool flag1,bool flag2) : base(C => C.Id == custodyId)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(C => C.CustodyItems);
            Include.Add(C => C.Recipients);
            Include.Add(C => C.Unit);
            IncludeStrings.Add($"{nameof(Custoday.CustodyItems)}.{nameof(CustodyItem.Item)}");
            IncludeStrings.Add($"{nameof(Custoday.CustodyItems)}.{nameof(CustodyItem.UnitExpenseLinks)}");
           
        }
    }
}
