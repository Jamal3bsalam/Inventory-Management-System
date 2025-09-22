using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.OrderSpecification
{
    public class OrderSpec:Specifications<Orders,int>
    {
        public OrderSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public OrderSpec(SpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.OrderNumber.ToLower().Contains(specParameters.Search)))
        {

            if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
            {
                ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
            }
            ApplyInclude();
        }

        public OrderSpec(OrderType orderType) : base(S => S.IsDeleted != true && S.OrderType == orderType.ToString())
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(O => O.OrderItems);
            Include.Add(O => O.StoreReleaseItems);
            IncludeStrings.Add($"{nameof(Orders.OrderItems)}.{nameof(OrderItems.SerialNumbers)}");
        }
    }
}
