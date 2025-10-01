using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.Store
{
    public class StoreReleaseSpecParameter
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; } = 1;
    }
}
