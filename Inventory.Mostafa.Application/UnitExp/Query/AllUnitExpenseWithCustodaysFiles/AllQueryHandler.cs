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
                    .Select(i => i.ItemId)
                    .Where(i => i.HasValue)
                    .Distinct()
                    .ToList();

            // ✅ نجيب كل Custodays مرة واحدة
            List<CustodayTransfers> relatedCustodays = new();
            TransferSpec custodaySpec;

            if (allExpenseItemIds.Any()) 
            {
                if (request.UnitId.HasValue)
                {
                    custodaySpec = new TransferSpec(request.UnitId.Value,allExpenseItemIds);
                }
                else
                {
                    custodaySpec = new TransferSpec(null,allExpenseItemIds);
                }
                relatedCustodays = (List<CustodayTransfers>)await _unitOfWork.Repository<CustodayTransfers, int>().GetAllWithSpecAsync(custodaySpec);
            }

            // ✅ Mapping من غير async
            var unitExpenseDtos = unitExpenses.Select(S =>
                {
                    var expenseItemIds = S.ExpenseItems
                        .Select(i => i.ItemId)
                        .Where(i => i.HasValue)
                        .ToList();

                    var custodaysForExpense = relatedCustodays
                        .Where(t => t.UnitId == S.UnitId && expenseItemIds.Contains(t.ItemId))
                        .ToList();

                    var oldRecipients = custodaysForExpense
                        .Where(c => c.OldRecipientId != S.RecipientsId)
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => c.OldRecipient!.Name)
                        .Distinct()
                        .ToList();

                    var custodaysFiles = custodaysForExpense
                        .Where(c => !string.IsNullOrEmpty(c.DocumentPath))
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => _configuration["BASEURL"] + c.DocumentPath)
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
                        CustodaysFiles = custodaysFiles
                    };
                }).ToList();

            var pagination = new Pagination<IEnumerable<UnitExpenseDetailsDto>>(parameter.PageSize,parameter.PageIndex,counts,unitExpenseDtos);

            return Result<Pagination<IEnumerable<UnitExpenseDetailsDto>>>.Success(pagination,"All Unit Expense Retrieved Successfully.");
        }
    }
}
