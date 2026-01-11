using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Query.AllItems
{
    public class AllItemsQuery : IRequest<Pagination<IEnumerable<ItemDto>>>
    {
        public int? pageSize { get; set; } = 10;
        public int? pageIndex { get; set; } = 1;
        public int? year { get; set; }
        //public string? OrderBy { get; set; } = "ItemsName";

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
