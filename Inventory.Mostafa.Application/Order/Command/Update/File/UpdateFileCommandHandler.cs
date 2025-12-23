using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Update.File
{
    public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, Result<string>>
    {
        private readonly IFileServices<Orders, int> _fileServices;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFileCommandHandler(IFileServices<Orders, int> fileServices, IUnitOfWork unitOfWork)
        {
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            if (request.File.Length == 0 || request.File == null) return Result<string>.Failure("No File Uploaded");

            if (request.Id.Value == null) return Result<string>.Failure("Please Enter A Valid Id.");
            var spec = new OrderSpec(request.Id.Value);
            var order = await _unitOfWork.Repository<Orders, int>().GetWithSpecAsync(spec);

            if (!string.IsNullOrEmpty(order.Attachment))
            {
                _fileServices.Delete(order.Attachment);
            }

            var fileName = _fileServices.Upload(request.File);
            if (fileName == null) return Result<string>.Failure("No File Uploaded");

            return Result<string>.Success(fileName);
        }
    }
}
