using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OpeningSpecification;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Update
{
    public class DeleteOpeningCommandHandler : IRequestHandler<DeleteOpeningCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOpeningCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteOpeningCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<string>.Failure("Please Enter A Valid Id.");

            var spec = new OpeningSpec(request.Id.Value);
            var openingStock = await _unitOfWork.Repository<OpeningStock, int>().GetWithSpecAsync(spec);

            if (openingStock == null) return Result<string>.Failure("No Opening Stock With This Id To Delete.");

            _unitOfWork.Repository<OpeningStock, int>().Delete(openingStock);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<string>.Failure("Faild To Delete Opening Stock");

            return Result<string>.Success("Opening Stock Deleted Sucessfully.");
        }
    }
}
