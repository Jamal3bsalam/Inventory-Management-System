using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Return.Command.Delete
{
    public class DeleteReturnCommandHandler : IRequestHandler<DeleteReturnCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteReturnCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteReturnCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0 || request.Id == null) return Result<string>.Failure("Please Enter Valid Id.");



            var returnSpec = new ReturnSpec(request.Id.Value);
            var returns = await _unitOfWork.Repository<Returns, int>().GetWithSpecAsync(returnSpec);
            if (returns == null) return Result<string>.Failure($"Return With This Id: {request.Id} Not Found");

            var unitExpenseSpec = new UnitExpenseSpec(returns.ExpenseId.Value);
            var unitExpense = await _unitOfWork.Repository<UnitExpense, int>().GetWithSpecAsync(unitExpenseSpec);
            if(unitExpense == null) return Result<string>.Failure($"UnitExpense with Id: {returns.ExpenseId} not found");


            var expenseItemSpec = new UnitExpenseItemSpec(returns.ExpenseId.Value);
            var expenseItem = await _unitOfWork.Repository<UnitExpenseItems,int>().GetAllWithSpecAsync(expenseItemSpec);
            if(expenseItem == null) return Result<string>.Failure($"No UnitExpenseItems found for Expense Id: {returns.ExpenseId}");


            var custodaySpec = new CustodaySpec(returns.RecipientsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday,int>().GetWithSpecAsync(custodaySpec);
            if(custoday == null) return Result<string>.Failure($"Custoday for recipient Id: {returns.RecipientsId} not found");


            var oldItem = expenseItem.FirstOrDefault(i => i.ItemId == returns.ItemId);
            oldItem.Quantity += returns.Quantity;
            _unitOfWork.Repository<UnitExpenseItems, int>().Update(oldItem);

            if (unitExpense.StoreReleaseId.HasValue)
            {
                var storSpec = new StoreItemSpec(unitExpense.StoreReleaseId.Value);
                var storeReleaseItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetAllWithSpecAsync(storSpec);
                var selectedStoreReleaseItem = storeReleaseItem.FirstOrDefault(i => i.ItemId == returns.ItemId);
                if (selectedStoreReleaseItem == null) return Result<string>.Failure("Faild to update Store Release Item Quantity.");
                if (storeReleaseItem != null)
                selectedStoreReleaseItem.Quantity += returns.Quantity;
                _unitOfWork.Repository<StoreReleaseItem, int>().Update(selectedStoreReleaseItem);

            }

            var custodayItems = custoday.CustodyItems.FirstOrDefault(i => i.ItemId == returns.ItemId);
            if (custodayItems == null)
            {
                var newCustodayItems = new CustodyItem()
                {
                    CustodyId = custoday.Id,
                    ItemId = returns.ItemId,
                    Quantity = returns.Quantity,
                };
                await _unitOfWork.Repository<CustodyItem, int>().AddAsync(newCustodayItems);
            }
            else
            {
                custodayItems.Quantity += returns.Quantity;
                _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
            }

            _unitOfWork.Repository<Returns,int>().Delete(returns);


            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Faild To Delete Returns");

            return Result<string>.Success("Returns Deleted Successfully");


        }
    }
}
