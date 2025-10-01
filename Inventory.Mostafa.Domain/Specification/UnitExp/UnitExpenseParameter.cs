using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitExp
{
    public class UnitExpenseParameter
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
