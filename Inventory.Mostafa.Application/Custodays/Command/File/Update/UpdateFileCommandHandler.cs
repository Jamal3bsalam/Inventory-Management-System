using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Command.File.Update
{
    public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices<CustodayTransfers, int> _fileServices;

        public UpdateFileCommandHandler(IUnitOfWork unitOfWork,IFileServices<CustodayTransfers,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }
        public async Task<Result<string>> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            if (request.File.Length == 0 || request.File == null) return Result<string>.Failure("No File Uploaded");

            if (request.Id.Value == null) return Result<string>.Failure("Please Enter A Valid Id.");
            var transfer = await _unitOfWork.Repository<CustodayTransfers, int>().GetByIdAsync(request.Id.Value);

            if (!string.IsNullOrEmpty(transfer.DocumentPath))
            {
                _fileServices.Delete(transfer.DocumentPath);
            }

            var fileName = _fileServices.Upload(request.File);
            if (fileName == null) return Result<string>.Failure("No File Uploaded");

            transfer.DocumentPath = $"\\Files\\CustodayTransfers\\{fileName}";
            _unitOfWork.Repository<CustodayTransfers,int>().Update(transfer);
            await _unitOfWork.CompleteAsync();

            return Result<string>.Success(fileName);
        }
    }
}
