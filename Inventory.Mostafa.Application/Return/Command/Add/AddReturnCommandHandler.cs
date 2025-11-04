using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Return.Command.Add
{
    public class AddReturnCommandHandler : IRequestHandler<AddReturnCommand, Result<ReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<Returns, int> _fileServices;

        public AddReturnCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration,IFileServices<Returns,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<ReturnDto>> Handle(AddReturnCommand request, CancellationToken cancellationToken)
        {

            var returns = new Returns();

            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit,int>().GetWithSpecAsync(unitSpec);
            if (unit == null) return Result<ReturnDto>.Failure($"Unit With Id: {request.UnitId} Not Fount");

            var recipintsSpec = new RecipintsSpec(request.RecipintsId.Value);
            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);
            if (recipints == null) return Result<ReturnDto>.Failure($"Recipints With Id: {request.RecipintsId} Not Fount");

            var custodaySpec = new CustodaySpec(request.RecipintsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

            var unitExpenseSpec = new UnitExpenseSpec(request.UnitExpenseId.Value);
            var unitExpense = await _unitOfWork.Repository<UnitExpense, int>().GetWithSpecAsync(unitExpenseSpec);
            if (unitExpense == null) return Result<ReturnDto>.Failure($"Unit Expense With Id: {request.UnitExpenseId} Not Fount");

            var expenseItemsSpec = new UnitExpenseItemSpec(unitExpense.Id);
            var expenseItems = await _unitOfWork.Repository<UnitExpenseItems, int>().GetAllWithSpecAsync(expenseItemsSpec);

            if (expenseItems == null) return Result<ReturnDto>.Failure($"Unit Expense Item For Unit Expense With Id: {request.UnitExpenseId} Not Fount");


            var selectedItem = expenseItems.FirstOrDefault(i => i.ItemId == request.ItemId);

            if (unitExpense.StoreReleaseId != null)
            {
                var storSpec = new StoreItemSpec(unitExpense.StoreReleaseId.Value);
                var storeReleaseItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetAllWithSpecAsync(storSpec);
                var selectedStoreReleaseItem = storeReleaseItem.FirstOrDefault(i => i.ItemId == request.ItemId);
                if (selectedStoreReleaseItem == null) return Result<ReturnDto>.Failure("Faild to update Store Release Item Quantity.");
                if (storeReleaseItem != null)
                    if(request.Quantity > selectedStoreReleaseItem.Quantity) return Result<ReturnDto>.Failure($"Return quantity cannot be greater than the released quantity(Available: {selectedStoreReleaseItem.Quantity}, Requested: {request.Quantity}).");
                        selectedStoreReleaseItem.Quantity -= request.Quantity;
                _unitOfWork.Repository<StoreReleaseItem,int>().Update(selectedStoreReleaseItem);
                if(selectedStoreReleaseItem.Quantity < 0)
                {
                    return Result<ReturnDto>.Failure($"The Returned Items Quantity Can Not be Grater Than Quantity In Unit Expense");
                }

            }

               await _unitOfWork.CompleteAsync();

            if (request.Document != null)
            {
                string fileName = _fileServices.Upload(request.Document);
                returns.DocumentPath = $"\\Files\\Returns\\{fileName}";
            }
            else
            {
                returns.DocumentPath = null;
            }
            returns.UnitId = request.UnitId;
            returns.RecipientsId = request.RecipintsId;
            returns.ExpenseId = request.UnitExpenseId;
            returns.Quantity = request.Quantity;
            returns.Reason = request.Reason;
            returns.ItemId = request.ItemId;

            await _unitOfWork.Repository<Returns,int>().AddAsync(returns);

           
            if (selectedItem == null) return Result<ReturnDto>.Failure("Faild to update Unit Expense Item Quantity.");

            var custodayItems = custoday?.CustodyItems?.FirstOrDefault(c => c.CustodyId == custoday.Id && c.ItemId == selectedItem.ItemId);
            if (custodayItems != null)
            {
                custodayItems.Quantity -= request.Quantity;
                _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
                if (custodayItems.Quantity <= 0) 
                {
                    _unitOfWork.Repository<CustodyItem, int>().Delete(custodayItems);
                }
            }

            if (request.Quantity > selectedItem.Quantity) return Result<ReturnDto>.Failure($"Return quantity cannot be greater than the released quantity(Available: {selectedItem.Quantity}, Requested: {request.Quantity}).");
            selectedItem.Quantity -= request.Quantity;
            _unitOfWork.Repository<UnitExpenseItems, int>().Update(selectedItem);
            if (selectedItem.Quantity < 0)
            {
                return Result<ReturnDto>.Failure($"The Returned Items Quantity Can Not be Grater Than Quantity In Unit Expense");
            }
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<ReturnDto>.Failure("Faild To Add Returns");

            var returnDto = new ReturnDto()
            {
                Id = returns.Id,
                UnitName = unit.UnitName,
                RecipintsName = recipints.Name,
                ItemName = selectedItem.Item.ItemsName,
                DocumentUrl = returns.DocumentPath != null ? _configuration["BASEURL"] + returns.DocumentPath : null,
                Quantity = returns.Quantity,
                Reason = returns.Reason,
            };

            return Result<ReturnDto>.Success(returnDto,"Returns Add Successfully");    
        }
    }
}
