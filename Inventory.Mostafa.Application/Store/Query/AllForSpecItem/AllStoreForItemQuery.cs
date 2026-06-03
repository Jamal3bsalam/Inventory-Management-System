using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;

namespace Inventory.Mostafa.Application.Store.Query.AllForSpecItem
{
    public class AllStoreForItemQuery : IRequest<Result<Pagination<IEnumerable<StoreReleaseDto>>>>
    {
        public int? UnitId { get; set; }
        public int? ItemId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; } = 1;
    }
}
