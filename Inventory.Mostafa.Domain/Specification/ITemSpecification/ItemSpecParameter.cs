using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.ITemSpecification
{
    public class ItemSpecParameter
    {
        public int? pageSize { get; set; }
        public int? pageIndex { get; set; } = 1;
        //public string? OrderBy { get; set; } = "ItemsName";

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
