using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.UnitExp.Query.Report
{
    public class ReportTimeQueryHandler : IRequestHandler<ReportTimeQuery, Result<IEnumerable<UnitExpensDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ReportTimeQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<IEnumerable<UnitExpensDto>>> Handle(ReportTimeQuery request, CancellationToken cancellationToken)
        {
            if (request.UnitId == null || request.UnitId == 0 || request.StartDate == null || request.EndDate == null) return Result<IEnumerable<UnitExpensDto>>.Failure("Please Fill The Fields With Valid Data");

            var unitExpenseSpec = new UnitExpenseSpec(request.UnitId.Value, request.StartDate, request.EndDate);
            var unitExpense = await _unitOfWork.Repository<UnitExpense,int>().GetAllWithSpecAsync(unitExpenseSpec);

            if (unitExpense == null) return Result<IEnumerable<UnitExpensDto>>.Failure($"Ther No Unit Expense For This Unit: {request.UnitId} between {request.StartDate} To {request.EndDate}");

            var unitExpenseDtos = unitExpense.Select(S => new UnitExpensDto()
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

            return Result<IEnumerable<UnitExpensDto>>.Success(unitExpenseDtos, "All Unit Expense Retrived Successfully.");

        }
    }
}
