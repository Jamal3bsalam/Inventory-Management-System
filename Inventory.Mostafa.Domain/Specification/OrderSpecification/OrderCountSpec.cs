using Inventory.Mostafa.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.OrderSpecification
{
    public class OrderCountSpec:Specifications<Orders,int>
    {
        public OrderCountSpec(SpecParameter specParameters) : base(U => U.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || U.OrderNumber.ToLower().Contains(specParameters.Search)))
        {
        }
    }
}
