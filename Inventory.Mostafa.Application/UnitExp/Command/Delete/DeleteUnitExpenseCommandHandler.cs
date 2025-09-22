using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
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

            unitExpense.IsDeleted = true;
            unitExpense.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<UnitExpense, int>().Update(unitExpense);
            var result = await _unitOfWork.CompleteAsync();

            if(result <= 0) Result<string>.Failure("Faild To Delete This Unit Expense");

            return Result<string>.Success("Unit Expense Deleted Successfully.");

        }
    }
}
