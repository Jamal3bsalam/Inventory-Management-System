using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.OpeningSpecification
{
    public class OpeningSpec:Specifications<OpeningStock,int>
    {
        public OpeningSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public OpeningSpec(SpecParameter specParameters) : base(S => (string.IsNullOrEmpty(specParameters.Search)) || S.Id.ToString().Contains(specParameters.Search))
        {

            if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
            {
                ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
            }
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(O => O.Items);
            Include.Add(O => O.SerialNumbers);
        }
    }
}
