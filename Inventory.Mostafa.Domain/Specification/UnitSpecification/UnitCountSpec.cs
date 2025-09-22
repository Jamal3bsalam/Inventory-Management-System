using Inventory.Mostafa.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitSpecification
{
    public class UnitCountSpec:Specifications<Unit,int>
    {
        public UnitCountSpec(SpecParameter specParameters) : base(U => U.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || U.UnitName.ToLower().Contains(specParameters.Search)))
        {
        }
    }
}
