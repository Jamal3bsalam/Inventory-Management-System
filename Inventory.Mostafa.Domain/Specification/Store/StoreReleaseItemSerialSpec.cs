using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreReleaseItemSerialSpec :Specifications<ReleaseItemSerialNumber,int>
    {
        public StoreReleaseItemSerialSpec(int id) : base(i => i.Id == id)
        {
        }

        public StoreReleaseItemSerialSpec(string serialNumbers) : base(s => (string.IsNullOrEmpty(serialNumbers) || s.SerialNumber.ToLower().Contains(serialNumbers)))
        {
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Include.Add(s => s.StoreReleaseItem);
        }
    }
}
