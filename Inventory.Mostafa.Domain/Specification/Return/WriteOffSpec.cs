using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Return
{
    public class WriteOffSpec:Specifications<WriteOff,int>
    {
        public WriteOffSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public WriteOffSpec(StoreReleaseSpecParameter parameter) : base(S => S.IsDeleted != true)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public WriteOffSpec(StoreReleaseSpecParameter parameter, bool flag) : base(S => S.IsDeleted != true && S.UnitId == parameter.UnitId)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public WriteOffSpec() : base(S => S.IsDeleted == true)
        {
            ApplyInclude();
        }
        public WriteOffSpec(int unitId,bool flag) : base(S => S.IsDeleted == true && S.UnitId == unitId)
        {
            ApplyInclude();
        }

        public WriteOffSpec(int returnId, bool flag1,bool flag2) : base(S => S.IsDeleted == true && S.ReturnId == returnId)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(r => r.Unit);
            Include.Add(r => r.Recipients);
            Include.Add(r => r.Returns);
            //Include.Add(r => r.Returns.Item);

        }
    }
}
