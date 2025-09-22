using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Add.Attachment
{
    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices<Orders, int> _fileServices;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, IFileServices<Orders, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
        }
        public async Task<Result<string>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            if (request.File.Length == 0 || request.File == null) return Result<string>.Failure("No File Uploaded");

            var fileName = _fileServices.Upload(request.File);
            if (fileName == null) return Result<string>.Failure("No File Uploaded");

            return Result<string>.Success(fileName);
        }
    }
}
