using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Query.AllUnitExpense
{
    public class UnitsExpensesQueryHandler : IRequestHandler<UnitsExpensesQuery, Result<Pagination<IEnumerable<UnitExpensDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UnitsExpensesQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<UnitExpensDto>>>> Handle(UnitsExpensesQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<UnitExpenseParameter>();
            parameter.Search = request.Search;
            var spec = request.UnitId == null ? new UnitExpenseSpec(parameter) : new UnitExpenseSpec(parameter, true);
            var count = request.UnitId == null ? new UnitExpenseCount(parameter) : new UnitExpenseCount(parameter, true);

            var unitExpenses = await _unitOfWork.Repository<UnitExpense, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<UnitExpense, int>().GetCountAsync(count);



            if (unitExpenses == null) return Result<Pagination<IEnumerable<UnitExpensDto>>>.Failure("Faild To Retrived All Unit Expense.");

            var unitExpenseDtos = unitExpenses.Select(S => new UnitExpensDto()
            {
                Id = S.Id,
                UnitName = S.Unit?.UnitName,
                RecipientsName = S.Recipients?.Name,
                ExpenseDate = S.ExpenseDate,
                DocumentNumber = S.DocumentNumber,
                ExpenseType = S.ExpenseType,
                AttachmentUrl = _configuration["BASEURL"] + S.AttachmentUrl,
                UnitExpenseItemsDtos = S.ExpenseItems.Select(i => new UnitExpensItemsDto()
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item?.ItemsName,
                    Quantity = i.Quantity
                }).ToList()
            });
            var pagintion = new Pagination<IEnumerable<UnitExpensDto>>(parameter.PageSize, parameter.PageIndex, counts, unitExpenseDtos);

            return Result<Pagination<IEnumerable<UnitExpensDto>>>.Success(pagintion, "All Unit Expense Retrived Successfully.");
        }
    }
}
