using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
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

            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit,int>().GetWithSpecAsync(unitSpec);
            if (unit == null) return Result<ReturnDto>.Failure($"Unit With Id: {request.UnitId} Not Fount");

            var recipintsSpec = new RecipintsSpec(request.RecipintsId.Value);
            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);
            if (recipints == null) return Result<ReturnDto>.Failure($"Recipints With Id: {request.RecipintsId} Not Fount");

            if (request.ReturnItems == null || !request.ReturnItems.Any()) return Result<ReturnDto>.Failure("Return items are required");

            var returns = new Returns
            {
                UnitId = request.UnitId.Value,
                RecipientsId = request.RecipintsId.Value,
                Reason = request.Reason
            };

            if (request.DocumentUrl != null)
            {
                returns.DocumentPath = $"\\Files\\Returns\\{request.DocumentUrl}";
            }


            foreach (var returnItem in request.ReturnItems)
            {
                var unitExpenseSpec = new UnitExpenseSpec(returnItem.UnitExpenseId);
                var unitExpense = await _unitOfWork
                    .Repository<UnitExpense, int>()
                    .GetWithSpecAsync(unitExpenseSpec);

                if (unitExpense == null)
                    return Result<ReturnDto>.Failure($"UnitExpense {returnItem.UnitExpenseId} not found");

                var expenseItemsSpec = new UnitExpenseItemSpec(unitExpense.Id);
                var expenseItems = await _unitOfWork
                    .Repository<UnitExpenseItems, int>()
                    .GetAllWithSpecAsync(expenseItemsSpec);

                var selectedItem = expenseItems
                    .FirstOrDefault(i => i.ItemId == returnItem.ItemId && i.Quantity != 0);

                if (selectedItem == null)
                    return Result<ReturnDto>.Failure("Item not found in UnitExpense");

                if (returnItem.Quantity > selectedItem.Quantity)
                    return Result<ReturnDto>.Failure(
                        $"Returned quantity exceeds available quantity (Available: {selectedItem.Quantity})"
                    );

                // Update UnitExpenseItem
                selectedItem.Quantity -= returnItem.Quantity;
                _unitOfWork.Repository<UnitExpenseItems, int>().Update(selectedItem);

                // Update Custody
                var custodaySpec = new CustodaySpec(request.RecipintsId.Value);
                var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

                var custodyItem = custoday?.CustodyItems?
                    .FirstOrDefault(c => c.ItemId == selectedItem.ItemId);

                if (custodyItem != null)
                {
                    custodyItem.Quantity -= returnItem.Quantity;

                    if (custodyItem.Quantity <= 0)
                        _unitOfWork.Repository<CustodyItem, int>().Delete(custodyItem);
                    else
                        _unitOfWork.Repository<CustodyItem, int>().Update(custodyItem);
                }

                // Store Release (لو موجود)
                if (unitExpense.StoreReleaseId != null)
                {
                    var storeSpec = new StoreItemSpec(unitExpense.StoreReleaseId.Value);
                    var storeItems = await _unitOfWork
                        .Repository<StoreReleaseItem, int>()
                        .GetAllWithSpecAsync(storeSpec);

                    var storeItem = storeItems
                        .FirstOrDefault(i => i.ItemId == returnItem.ItemId);

                    if (storeItem == null)
                        return Result<ReturnDto>.Failure("Store release item not found");

                    if (returnItem.Quantity > storeItem.Quantity)
                        return Result<ReturnDto>.Failure("Returned quantity exceeds store quantity");

                    storeItem.Quantity -= returnItem.Quantity;
                    _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);
                }

                // 🔹 Add ReturnItem Entity
                returns?.ReturnItems?.Add(new ReturnItem
                {
                    ItemId = returnItem.ItemId,
                    UnitExpenseId = returnItem.UnitExpenseId,
                    Quantity = returnItem.Quantity
                });
            }

            await _unitOfWork.Repository<Returns, int>().AddAsync(returns);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<ReturnDto>.Failure("Failed to create return");

            var returnDto = new ReturnDto
            {
                Id = returns.Id,
                UnitName = unit.UnitName,
                RecipintsName = recipints.Name,
                DocumentUrl = returns.DocumentPath,
                Reason = returns.Reason,
                ItemResponseDtos = returns.ReturnItems.Select(ri => new ReturnItemResponseDto
                {
                    ExpenseId = ri.UnitExpenseId,
                    DocumentNumber = ri.UnitExpense.DocumentNumber,
                    DocumentUrl = string.IsNullOrEmpty(ri.UnitExpense?.AttachmentUrl) ? null : _configuration["BASEURL"] + ri.UnitExpense?.AttachmentUrl,
                    ItemName = ri.Item.ItemsName,
                    Quantity = ri.Quantity
                }).ToList()
            };

            return Result<ReturnDto>.Success(returnDto,"Returns Add Successfully");    
        }
    }
}
