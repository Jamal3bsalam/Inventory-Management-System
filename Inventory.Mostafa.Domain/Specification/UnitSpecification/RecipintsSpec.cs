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
        public RecipintsSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }

        public RecipintsSpec(int unitId,bool flag) : base(i => i.UnitId == unitId)
        {
            AddOrderBy(r => r.Name);
            ApplyInclude();
        }

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
