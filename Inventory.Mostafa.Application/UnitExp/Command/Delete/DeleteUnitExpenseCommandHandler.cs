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

namespace Inventory.Mostafa.Application.UnitExp.Command.Delete
{
    public class DeleteUnitExpenseCommandHandler : IRequestHandler<DeleteUnitExpenseCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUnitExpenseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteUnitExpenseCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null || request.Id == 0) return Result<string>.Failure("Please Enter A Valid Id.");


            var spec = new UnitExpenseSpec(request.Id.Value);
            var unitExpense = await _unitOfWork.Repository<UnitExpense,int>().GetWithSpecAsync(spec);
            if (unitExpense == null) return Result<string>.Failure("There Is No Unit Expense With This Id");

            var custodaySpec = new CustodaySpec(unitExpense.RecipientsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday,int>().GetWithSpecAsync(custodaySpec);

            if (custoday != null)
            {
                foreach (var item in unitExpense.ExpenseItems)
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

            if (unitExpense.StoreReleaseId != null)
            {

                var storeSpec = new StoreReleaseSpec(unitExpense.StoreReleaseId.Value);
                var storeRelease = await _unitOfWork.Repository<StoreRelease, int>().GetWithSpecAsync(storeSpec);

                if (storeRelease != null)
                {
                    storeRelease.DeletedAt = DateTime.UtcNow;
                    storeRelease.IsDeleted = true;
                    _unitOfWork.Repository<StoreRelease, int>().Update(storeRelease);
                }

            }
            

            unitExpense.IsDeleted = true;
            unitExpense.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<UnitExpense, int>().Update(unitExpense);
            var result = await _unitOfWork.CompleteAsync();

            
            if (result <= 0) return Result<string>.Failure("Faild To Delete This Unit Expense");

            return Result<string>.Success("Unit Expense Deleted Successfully.");

        }
    }
}
