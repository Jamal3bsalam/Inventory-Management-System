using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.ITemSpecification
{
    public class ItemSpecParameter
    {
        public int? pageSize { get; set; } = 10;
        public int? pageIndex { get; set; } = 1;
        public int? year { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
