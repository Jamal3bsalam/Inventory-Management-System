using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            returns.IsDeleted = true;
            returns.DeletedAt = DateTime.Now;

            _unitOfWork.Repository<Returns,int>().Update(returns);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<string>.Failure("Faild To Delete Returns");

            return Result<string>.Success("Returns Delete Successfully");


        }
    }
}
