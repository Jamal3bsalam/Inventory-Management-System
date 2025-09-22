using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Store.Command.Add.NewRecipints
{
    public class AddNewRecipintsCommandHandler : IRequestHandler<AddNewRecipintsCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddNewRecipintsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(AddNewRecipintsCommand request, CancellationToken cancellationToken)
        {
            if (request.UnitId == null) return Result<string>.Failure("Please Enter A Valid Unit Id");

            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit,int>().GetWithSpecAsync(unitSpec);

            if (unit == null) return Result<string>.Failure($"There Is No Unit With This Id: {request.UnitId}");
            var recipints = new Recipients() { UnitId = request.UnitId , Name = request.RecipintsName };
            unit.Recipients.Add(recipints);

            _unitOfWork.Repository<Unit, int>().Update(unit);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Faild To Add New Recipints");

            return Result<string>.Success("A New Recipints Added Successfully.");

        }
    }
}
