using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Application.Units.Command.Delete
{
    public class DeleteRecipintsCommandHandler : IRequestHandler<DeleteRecipintsCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecipintsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteRecipintsCommand request, CancellationToken cancellationToken)
        {
            if ((request.UnitId == 0 || request.UnitId == null) && (request.RecipintsId == 0 || request.RecipintsId == null)) return Result<string>.Failure("Please Enter Valid Data");

            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(unitSpec);

            if (unit == null) return Result<string>.Failure($"There No Unit With This Id: {request.UnitId}");

            var recipints = unit.Recipients.FirstOrDefault(r => r.Id == request.RecipintsId);   

            if (recipints == null) return Result<string>.Failure($"There No Recipints With This Id: {request.RecipintsId} For Unit With This Id: {request.UnitId}");

            _unitOfWork.Repository<Recipients,int>().Delete(recipints);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<string>.Failure("Faild To Delete Recipints.");

            return Result<string>.Success("Recipients Deleted Successfully.");
        }
    }
}
