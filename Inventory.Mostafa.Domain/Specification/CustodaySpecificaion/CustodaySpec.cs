using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.CustodaySpecificaion
{
    public class CustodaySpec:Specifications<Custoday,int>
    {
        public CustodaySpec(int recipintsId) : base(C => C.RecipientsId == recipintsId)
        {
            ApplyInclude();
        }

        //public ItemSpec(SpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.ItemsName.ToLower().Contains(specParameters.Search)))
        //{

        //    if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
        //    {
        //        ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
        //    }
        //}

        private void ApplyInclude()
        {
            Include.Add(C => C.CustodyItems);
        }
    }
}
