using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq;
namespace Inventory.Mostafa.Application.UnitExp.Query.AllUnitExpenseWithCustodaysFiles
{
    internal class AllQueryHandler : IRequestHandler<AllQuery, Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>> Handle(AllQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<UnitExpenseParameter>();
            parameter.Search = request.Search;
            var spec = request.UnitId == null ? new UnitExpenseSpec(parameter) : new UnitExpenseSpec(parameter, true);
            var count = request.UnitId == null ? new UnitExpenseCount(parameter) : new UnitExpenseCount(parameter, true);


            var unitExpenses = await _unitOfWork.Repository<UnitExpense, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<UnitExpense, int>().GetCountAsync(count);



            if (unitExpenses == null) return Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>.Failure("Faild To Retrived All Unit Expense.");

            var allExpenseItemIds = unitExpenses
                    .SelectMany(u => u.ExpenseItems)
                    .Select(i => i.Id)
                    .ToList();

            // ✅ نجيب كل Custodays مرة واحدة
            List<CustodyItemUnitExpense> custodyItemLinks = new();
            TransferSpec custodaySpec;

            if (allExpenseItemIds.Any()) 
            {
                var custodayExpenseSpec = new CustodyItemUnitExpenseSpec(allExpenseItemIds);
                custodyItemLinks = (List<CustodyItemUnitExpense>)await _unitOfWork.Repository<CustodyItemUnitExpense, int>().GetAllWithSpecAsync(custodayExpenseSpec);
            }

            // ✅ Mapping من غير async
            var unitExpenseDtos = unitExpenses.Select(S =>
                {
                    var expenseItemIds = S.ExpenseItems
                        .Select(i => i.Id)
                        .ToList();


                    var oldRecipients = custodyItemLinks
                        .Where(l => expenseItemIds.Contains((int)l.UnitExpenseItemId) && l.CustodyItem.Custody.RecipientsId != S.RecipientsId)
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.CustodyItem?.Custody?.Recipients?.Name)
                        .Distinct()
                        .ToList();

                    return new UnitExpenseDetailsDto
                    {
                        Id = S.Id,
                        UnitName = S.Unit?.UnitName,
                        RecipientsName = S.Recipients?.Name,
                        ExpenseDate = S.ExpenseDate,
                        DocumentNumber = S.DocumentNumber,
                        ExpenseType = S.ExpenseType,
                        AttachmentUrl = _configuration["BASEURL"] + S.AttachmentUrl,

                        UnitExpenseItemsDtos = S.ExpenseItems.Select(i => new UnitExpensItemsDto
                        {
                            ItemId = i.ItemId,
                            ItemName = i.Item?.ItemsName,
                            Quantity = i.Quantity
                        }).ToList(),

                        OldRecipintsRecipients = oldRecipients,
                    };
                }).ToList();

            var pagination = new Pagination<IEnumerable<UnitExpenseDetailsDto>>(parameter.PageSize,parameter.PageIndex,counts,unitExpenseDtos);

            return Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>.Success(pagination,"All Unit Expense Retrieved Successfully.");
        }
    }
}
