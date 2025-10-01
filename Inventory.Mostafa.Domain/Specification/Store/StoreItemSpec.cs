using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreItemSpec:Specifications<StoreReleaseItem,int>
    {
        public StoreItemSpec(int id) : base(i => i.StoreReleaseId == id)
        {
            ApplyInclude();
        }

        public StoreItemSpec(int id,bool flag) : base(i => i.Id == id)
        {
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Include.Add(s => s.StoreRelease);
            Include.Add(O => O.Items);
            Include.Add(s => s.Order);
            Include.Add(s => s.OrderItem);
        }

    }
}
