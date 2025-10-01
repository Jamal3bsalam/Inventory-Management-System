using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Return
{
    public class ReturnCount:Specifications<Returns,int>
    {
        public ReturnCount(StoreReleaseSpecParameter specParameter) : base(U => U.IsDeleted != true)
        {
        }

        public ReturnCount(StoreReleaseSpecParameter specParameter, bool flag) : base(U => U.IsDeleted != true && U.UnitId == specParameter.UnitId)
        {
        }
    }
}
