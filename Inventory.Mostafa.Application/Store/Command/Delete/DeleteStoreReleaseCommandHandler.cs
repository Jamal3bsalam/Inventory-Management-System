using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Delete
{
    public class DeleteStoreReleaseCommandHandler : IRequestHandler<DeleteStoreReleaseCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStoreReleaseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteStoreReleaseCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<string>.Failure("Please Enter A Valid Id.");

            var spec = new StoreReleaseSpec(request.Id.Value);
            var storeRelease = await _unitOfWork.Repository<StoreRelease,int>().GetWithSpecAsync(spec);

            var expenseSpec = new UnitExpenseSpec(request.Id.Value,true);
            var expense = await _unitOfWork.Repository<UnitExpense, int>().GetWithSpecAsync(expenseSpec);
            if (expense == null) return Result<string>.Failure($"No Store Release With This Id: {request.Id} In Units Expense");

            var custodaySpec = new CustodaySpec(storeRelease.RecipientsId);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

            if (storeRelease == null) return Result<string>.Failure("There Is No StoreRelease With This Id");

            storeRelease.IsDeleted = true;
            storeRelease.DeletedAt = DateTime.UtcNow;

            expense.IsDeleted = true;
            expense.DeletedAt = DateTime.UtcNow;

            if (custoday != null)
            {
                foreach (var item in storeRelease.StoreReleaseItems)
                {
                    var custodayItems = custoday.CustodyItems.FirstOrDefault(c => c.CustodyId == custoday.Id && c.ItemId == item.ItemId);

                    if (custodayItems != null)
                    {
                        custodayItems.Quantity -= item.Quantity;
                        if (custodayItems.Quantity <= 0)
                        {
                            _unitOfWork.Repository<CustodyItem, int>().Delete(custodayItems);
                        }
                        else
                        {
                            _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
                        }
                    }
                }

                if (!custoday.CustodyItems.Any())
                {
                    _unitOfWork.Repository<Custoday, int>().Delete(custoday);
                }
                else
                {
                    _unitOfWork.Repository<Custoday, int>().Update(custoday);
                }
            }

            _unitOfWork.Repository<StoreRelease,int>().Update(storeRelease); 
            _unitOfWork.Repository<UnitExpense,int>().Update(expense); 
            
           var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<string>.Failure("Faild To Delete Order");

            return Result<string>.Success("StoreRelease Deleted Sucessfully.", "");
        }
    }
}
