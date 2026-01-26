using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using MediatR;

namespace Inventory.Mostafa.Application.Custodays.Command.Update
{
    public class UpdateTransferCommandHandler : IRequestHandler<UpdateTransferCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices<CustodayTransfers, int> _fileServices;

        public UpdateTransferCommandHandler(IUnitOfWork unitOfWork,IFileServices<CustodayTransfers,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }
        public async Task<Result<string>> Handle(UpdateTransferCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.Value == null) return Result<string>.Failure("Please Enter A Valid Id.");
            var transfer = await _unitOfWork.Repository<CustodayTransfers, int>().GetByIdAsync(request.Id.Value);

            if(transfer == null) return Result<string>.Failure($"No Transfer Found With This Id:{request.Id}");

            if(request.File?.Length > 0 && request.File.FileName != null)
            {
                if (!string.IsNullOrEmpty(transfer.DocumentPath))
                {
                    _fileServices.Delete(transfer.DocumentPath);
                }

                var fileName = _fileServices.Upload(request.File);
                if (fileName == null) return Result<string>.Failure("No File Uploaded");

                transfer.DocumentPath = $"\\Files\\CustodayTransfers\\{fileName}";
            }

            if(!string.IsNullOrEmpty(request.HijriDate))
            {
                transfer.TransactionHijriDate = request.HijriDate;
            }
            
            _unitOfWork.Repository<CustodayTransfers,int>().Update(transfer);
            await _unitOfWork.CompleteAsync();

            return Result<string>.Success("Transfer Updated Successfully");
        }
    }
}
