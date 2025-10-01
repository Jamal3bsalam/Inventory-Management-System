using Inventory.Mostafa.Domain.Entities.Store;
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
    public class UnitExpenseSpec : Specifications<UnitExpense,int>
    {

        public UnitExpenseSpec(int id) : base(i => i.Id == id)
        {
            ApplyInclude();
        }
        public UnitExpenseSpec(UnitExpenseParameter parameter) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(parameter.Search) || S.DocumentNumber.ToLower().Contains(parameter.Search)))
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        public UnitExpenseSpec(UnitExpenseParameter parameter, bool flag) : base(S => S.IsDeleted != true && S.UnitId == parameter.UnitId && (string.IsNullOrEmpty(parameter.Search) || S.DocumentNumber.ToLower().Contains(parameter.Search)))
        {

            if (parameter.PageIndex.HasValue && parameter.PageSize.HasValue)
            {
                ApplyPagination((int)(parameter.PageSize * (parameter.PageIndex - 1)), (int)parameter.PageSize);
            }
            ApplyInclude();
        }

        //public OrderSpec(SpecParameter specParameters) : base(S => S.IsDeleted != true && (string.IsNullOrEmpty(specParameters.Search) || S.OrderNumber.ToLower().Contains(specParameters.Search)))
        //{

        //    if (specParameters.pageIndex.HasValue && specParameters.pageSize.HasValue)
        //    {
        //        ApplyPagination(specParameters.pageSize.Value * (specParameters.pageIndex.Value - 1), specParameters.pageSize.Value);
        //    }
        //    ApplyInclude();
        //}
        private void ApplyInclude()
        {
            Include.Add(S => S.Unit);
            Include.Add(O => O.Recipients);
            Include.Add(sr => sr.ExpenseItems);
            IncludeStrings.Add($"{nameof(UnitExpense.ExpenseItems)}.{nameof(UnitExpenseItems.Item)}");
        }
    }
}
