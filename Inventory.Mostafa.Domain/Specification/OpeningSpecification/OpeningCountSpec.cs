using Inventory.Mostafa.Domain.Entities.Opening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.OpeningSpecification
{
    public class OpeningCountSpec:Specifications<OpeningStock,int>
    {
        public OpeningCountSpec(SpecParameter specParameters) : base(U => (string.IsNullOrEmpty(specParameters.Search)) || U.Id.ToString().Contains(specParameters.Search))
        {
        }
    }
}
