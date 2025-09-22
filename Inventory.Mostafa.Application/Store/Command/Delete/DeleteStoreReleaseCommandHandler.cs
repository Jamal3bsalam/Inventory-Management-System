using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
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

            if (storeRelease == null) return Result<string>.Failure("There Is No StoreRelease With This Id");

            storeRelease.IsDeleted = true;
            storeRelease.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<StoreRelease,int>().Update(storeRelease);    
           var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<string>.Failure("Faild To Delete Order");

            return Result<string>.Success("StoreRelease Deleted Sucessfully.", "");
        }
    }
}
