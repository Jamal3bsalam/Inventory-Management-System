using Inventory.Mostafa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.ITemSpecification
{
    public class ItemSpec: Specifications<Items,int>
    {
        public ItemSpec(int id):base(i => i.Id == id)
        {
        }

        public ItemSpec(ItemSpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.ItemsName.ToLower().Contains(specParameters.Search)))
        {
            AddOrderBy(I => I.ItemsName);

            if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
            {
                ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
            }
        }

    }
}
