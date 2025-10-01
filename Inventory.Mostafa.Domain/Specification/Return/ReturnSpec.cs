using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Return
{
    public class ReturnSpec:Specifications<Returns,int>
    {
        public ReturnSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public ReturnSpec(StoreReleaseSpecParameter parameter) : base(S => S.IsDeleted != true)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public ReturnSpec(StoreReleaseSpecParameter parameter,bool flag) : base(S => S.IsDeleted != true && S.UnitId == parameter.UnitId)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public ReturnSpec() : base(S => S.IsDeleted == true)
        {
            ApplyInclude();
        }
        public ReturnSpec(int unitId, bool flag) : base(S => S.IsDeleted == true && S.UnitId == unitId)
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(r => r.Unit);
            Include.Add(r => r.StoreReleaseItem);
            Include.Add(r => r.Recipients);
            Include.Add(r => r.WriteOffs);
            IncludeStrings.Add($"{nameof(Returns.StoreReleaseItem)}.{nameof(StoreReleaseItem.OrderItem)}");

        }
    }
}
