using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification
{
    public class ItemCountSpecifications:Specifications<Items,int>
    {
        public ItemCountSpecifications(ItemSpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.ItemsName.ToLower().Contains(specParameters.Search)))
        {
        }
    }
}
