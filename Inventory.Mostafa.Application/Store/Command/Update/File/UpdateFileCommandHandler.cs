using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Update.File
{
    public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, Result<string>>
    {
        private readonly IFileServices<UnitExpense, int> _fileServices;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFileCommandHandler(IFileServices<UnitExpense, int> fileServices, IUnitOfWork unitOfWork)
        {
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            if (request.File.Length == 0 || request.File == null) return Result<string>.Failure("No File Uploaded");

            if (request.Id.Value == null) return Result<string>.Failure("Please Enter A Valid Id.");
            var spec = new StoreReleaseSpec(request.Id.Value);
            var storeRelease = await _unitOfWork.Repository<StoreRelease, int>().GetWithSpecAsync(spec);

            if (!string.IsNullOrEmpty(storeRelease.DocumentPath))
            {
                _fileServices.Delete(storeRelease.DocumentPath);
            }

            var fileName = _fileServices.Upload(request.File);
            if (fileName == null) return Result<string>.Failure("No File Uploaded");

            return Result<string>.Success(fileName);
        }
    }
}
