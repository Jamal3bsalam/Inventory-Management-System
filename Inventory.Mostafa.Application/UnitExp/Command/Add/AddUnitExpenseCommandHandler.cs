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

namespace Inventory.Mostafa.Application.UnitExp.Command.Add
{
    public class AddUnitExpenseCommandHandler : IRequestHandler<AddUnitExpenseCommand, Result<UnitExpensDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AddUnitExpenseCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<UnitExpensDto>> Handle(AddUnitExpenseCommand request, CancellationToken cancellationToken)
        {
            if(request.UnitId == 0 || request.RecipientsId == 0 || request.ExpenseType == null || request.ExpenseDate == null || string.IsNullOrEmpty(request.AttachmentUrl) || request.UnitExpenseItemsDtos.Count == 0)
            { return Result<UnitExpensDto>.Failure("Please Enter The Fileds With The Right Data"); }

            var unitExpense = new UnitExpense()
            {
                UnitId = request.UnitId,
                RecipientsId = request.RecipientsId,
                ExpenseType = request.ExpenseType,
                ExpenseDate = request.ExpenseDate,
                DocumentNumber = request.DocumentNumber,
                AttachmentUrl = $"\\Files\\UnitExpense\\{request.AttachmentUrl}",
                ExpenseItems = request.UnitExpenseItemsDtos.Select(u => new UnitExpenseItems() {ItemId = u.ItemId,Quantity = u.Quantity}).ToList()
            };

            await _unitOfWork.Repository<UnitExpense,int>().AddAsync(unitExpense);
            var result = await _unitOfWork.CompleteAsync();

            var spec = new UnitExpenseSpec(unitExpense.Id);
            var unitExp = await _unitOfWork.Repository<UnitExpense,int>().GetWithSpecAsync(spec);
            if (unitExp == null) return Result<UnitExpensDto>.Failure("Faild To Save This Unit Expense");

            var unitExpenseDto = new UnitExpensDto()
            {
                Id = unitExp.Id,
                UnitName = unitExp?.Unit?.UnitName,
                RecipientsName = unitExp?.Recipients?.Name,
                ExpenseType = unitExpense?.ExpenseType,
                AttachmentUrl = _configuration["BASEURL"] + unitExpense?.AttachmentUrl,
                DocumentNumber = unitExpense?.DocumentNumber,
                ExpenseDate = unitExpense?.ExpenseDate,
                UnitExpenseItemsDtos = unitExp?.ExpenseItems?.Select(u => new UnitExpensItemsDto() { ItemId = u.ItemId, ItemName = u.Item?.ItemsName ,Quantity = u.Quantity }).ToList()
            };

            if (result <= 0) return Result<UnitExpensDto>.Failure("Faild To Save This Unit Expense");

            return Result<UnitExpensDto>.Success(unitExpenseDto, "Unit Expense Saved Successfully");
             
        }
    }
}
