using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreReleaseSpec:Specifications<StoreRelease,int>
    {
        public StoreReleaseSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public StoreReleaseSpec(StoreReleaseSpecParameter parameter) : base(S => S.IsDeleted != true)
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue )
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public StoreReleaseSpec(StoreReleaseSpecParameter parameter,bool flag) : base(S => S.IsDeleted != true && S.UnitId == parameter.UnitId)
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
            Include.Add(sr => sr.StoreReleaseItems);
            IncludeStrings.Add($"{nameof(StoreRelease.StoreReleaseItems)}.{nameof(StoreReleaseItem.Items)}");
            IncludeStrings.Add($"{nameof(StoreRelease.StoreReleaseItems)}.{nameof(StoreReleaseItem.Order)}");
            IncludeStrings.Add($"{nameof(StoreRelease.StoreReleaseItems)}.{nameof(StoreReleaseItem.SerialNumbers)}");
        }
    
    }
}
