using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Units.Command.Delete
{
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUnitCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0) return Result<string>.Failure("Please fill all required fields correctly.");

            var spec = new UnitSpec(request.Id.Value);
            var unit = await _unitOfWork.Repository<Unit,int>().GetWithSpecAsync(spec);

            if (unit == null) return Result<string>.Failure("There Is No Unit With This Id.");

            var hasStoreRelease = unit.StoreReleases.Any();
            if (hasStoreRelease)
            {
                unit.IsDeleted = true;
                unit.DeletedAt = DateTime.UtcNow;
                foreach (var recipints in unit.Recipients)
                {
                    recipints.IsDeleted = true;
                    recipints.DeletedAt = DateTime.UtcNow;
                }
                _unitOfWork.Repository<Unit, int>().Update(unit);
            }
            else
            {
                _unitOfWork.Repository<Unit, int>().Delete(unit);
            }
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) Result<UnitDto>.Failure("Faild To Update This Item.");

            return Result<string>.Success("Unit Deleted Successfully.");

        }
    }
}
