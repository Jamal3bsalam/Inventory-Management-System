using Inventory.Mostafa.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitSpecification
{
    public class UnitSpec : Specifications<Unit,int>
    {
        public UnitSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public UnitSpec(SpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.UnitName.ToLower().Contains(specParameters.Search)))
        {

            if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
            {
                ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
            }

            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(U => U.Recipients);
            Include.Add(U => U.StoreReleases);
        }
    }
}
