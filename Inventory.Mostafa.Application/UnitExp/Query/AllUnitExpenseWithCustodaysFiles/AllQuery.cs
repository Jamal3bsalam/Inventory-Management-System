using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Shared;
using MediatR;

namespace Inventory.Mostafa.Application.UnitExp.Query.AllUnitExpenseWithCustodaysFiles
{
    public class AllQuery:IRequest<Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
