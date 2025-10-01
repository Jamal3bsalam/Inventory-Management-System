using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.CustodaySpecificaion
{
    public class TransferCount:Specifications<CustodayTransfers,int>
    {
        public TransferCount(StoreReleaseSpecParameter specParameter)
        {
        }

        public TransferCount(StoreReleaseSpecParameter specParameter, bool flag) : base(U => U.UnitId == specParameter.UnitId)
        {
        }
    }
}
