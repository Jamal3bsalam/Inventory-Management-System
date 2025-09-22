using Inventory.Mostafa.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreReleaseCountSpec:Specifications<StoreRelease,int>
    {
        public StoreReleaseCountSpec(StoreReleaseSpecParameter specParameter) : base(U => U.IsDeleted != true)
        {
        }

        public StoreReleaseCountSpec(StoreReleaseSpecParameter specParameter,bool flag) : base(U => U.IsDeleted != true && U.UnitId == specParameter.UnitId)
        {
        }
    }
}
