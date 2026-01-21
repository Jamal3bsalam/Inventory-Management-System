using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
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

            var returnItemSpec = new ReturnItemSpec(request.Id.Value);
            var returnItems = await _unitOfWork.Repository<ReturnItem, int>().GetAllWithSpecAsync(returnItemSpec);

            var custodaySpec = new CustodaySpec(returns.RecipientsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday,int>().GetWithSpecAsync(custodaySpec);
            if(custoday == null) return Result<string>.Failure($"Custoday for recipient Id: {returns.RecipientsId} not found");


            foreach (var returnItem in returns.ReturnItems)
            {
                // 1️⃣ Unit Expense
                var unitExpenseSpec = new UnitExpenseSpec(returnItem.UnitExpenseId);
                var unitExpense = await _unitOfWork.Repository<UnitExpense, int>().GetWithSpecAsync(unitExpenseSpec);

                if (unitExpense == null) return Result<string>.Failure($"UnitExpense {returnItem.UnitExpenseId} Not Found");

                // 2️⃣ UnitExpenseItems
                var expenseItemSpec = new UnitExpenseItemSpec(returnItem.UnitExpenseId);
                var expenseItems = await _unitOfWork.Repository<UnitExpenseItems, int>().GetAllWithSpecAsync(expenseItemSpec);

                var oldItem = expenseItems.FirstOrDefault(i => i.ItemId == returnItem.ItemId);

                if (oldItem == null) return Result<string>.Failure("UnitExpenseItem Not Found");

                oldItem.Quantity += returnItem.Quantity;
                _unitOfWork.Repository<UnitExpenseItems, int>().Update(oldItem);

                // 3️⃣ Store Release
                if (unitExpense.StoreReleaseId.HasValue)
                {
                    var storeSpec = new StoreItemSpec(unitExpense.StoreReleaseId.Value);
                    var storeItems = await _unitOfWork.Repository<StoreReleaseItem, int>()
                                                       .GetAllWithSpecAsync(storeSpec);

                    var storeItem = storeItems.FirstOrDefault(i => i.ItemId == returnItem.ItemId);

                    if (storeItem == null) return Result<string>.Failure("StoreReleaseItem Not Found");

                    storeItem.Quantity += returnItem.Quantity;
                    _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);
                }

                // 4️⃣ Custody Items
                var custodyItem = custoday?.CustodyItems?.FirstOrDefault(i => i.ItemId == returnItem.ItemId);

                if (custodyItem == null)
                {
                    await _unitOfWork.Repository<CustodyItem, int>().AddAsync(
                        new CustodyItem
                        {
                            CustodyId = custoday.Id,
                            ItemId = returnItem.ItemId,
                            Quantity = returnItem.Quantity
                        });
                }
                else
                {
                    custodyItem.Quantity += returnItem.Quantity;
                    _unitOfWork.Repository<CustodyItem, int>().Update(custodyItem);
                }
            }

            // 🧨 Delete Return (Cascade will delete ReturnItems)
            _unitOfWork.Repository<ReturnItem, int>().DeleteRange(returnItems);
            _unitOfWork.Repository<Returns, int>().Delete(returns);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Failed To Delete Return");

            return Result<string>.Success("Return Deleted Successfully");
        }
    }
}
