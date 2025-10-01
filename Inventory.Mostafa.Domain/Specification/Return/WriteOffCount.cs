using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Return
{
    public class WriteOffCount:Specifications<WriteOff,int>
    {
        public WriteOffCount(StoreReleaseSpecParameter specParameter) : base(U => U.IsDeleted != true)
        {
        }

        public WriteOffCount(StoreReleaseSpecParameter specParameter, bool flag) : base(U => U.IsDeleted != true && U.UnitId == specParameter.UnitId)
        {
        }
    }
}
