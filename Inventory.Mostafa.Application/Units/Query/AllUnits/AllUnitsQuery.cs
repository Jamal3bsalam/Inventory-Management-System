using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Query.AllUnits
{
    public class AllUnitsQuery:IRequest<Pagination<IEnumerable<UnitDto>>>
    {
        public int? pageSize { get; set; } = 10;
        public int? pageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
