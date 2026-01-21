using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Specification.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification.UnitExp
{
    public class UnitExpenseCount : Specifications<UnitExpense,int>
    {
        public UnitExpenseCount(UnitExpenseParameter specParameter) : base(U => U.IsDeleted != true && (string.IsNullOrEmpty(specParameter.Search) || U.DocumentNumber.ToLower().Contains(specParameter.Search)) && (specParameter.RecipintId == null || U.Recipients.Id == specParameter.RecipintId))
        {
        }

        public UnitExpenseCount(UnitExpenseParameter specParameter, bool flag) : base(U => U.IsDeleted != true && U.UnitId == specParameter.UnitId && (string.IsNullOrEmpty(specParameter.Search) || U.DocumentNumber.ToLower().Contains(specParameter.Search)))
        {
        }

        //public OrderCountSpec(SpecParameter specParameters) : base(U => U.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || U.OrderNumber.ToLower().Contains(specParameters.Search)))
        //{
        //}
    }
}
