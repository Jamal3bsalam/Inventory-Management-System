using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Query.AllUnitExpense
{
    public class UnitsExpensesQuery:IRequest<Result<Pagination<IEnumerable<UnitExpensDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        public int? RecipintId { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
