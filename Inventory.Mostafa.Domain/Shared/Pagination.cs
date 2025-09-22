using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Shared
{
    public class Pagination<TEntity>
    {
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        public int? Count { get; set; }
        public TEntity Data { get; set; }

        public Pagination(int? pageSize, int? pageIndex, int? count, TEntity data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
    }
}
