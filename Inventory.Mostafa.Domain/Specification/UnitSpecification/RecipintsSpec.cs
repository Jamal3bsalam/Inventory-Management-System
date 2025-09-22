using Inventory.Mostafa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitSpecification
{
    public class RecipintsSpec:Specifications<Recipients,int>
    {
        public RecipintsSpec(string search) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(search) || S.Name.ToLower().Contains(search)))
        {
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Include.Add(R => R.Unit);
        }
    }
}
