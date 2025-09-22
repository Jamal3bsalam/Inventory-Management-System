using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.OrderSpecification
{
    public class OpeningSpecParameter
    {
        public int? pageSize { get; set; }
        public int? pageIndex { get; set; } = 1;
    }
}
