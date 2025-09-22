using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Query.AllStoreRelease
{
    public class StoreReleasesQuery:IRequest<Result<Pagination<IEnumerable<StoreReleaseDto>>>>
    {
        public int? UnitId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}
