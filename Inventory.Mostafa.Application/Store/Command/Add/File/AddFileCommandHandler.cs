using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Command.Add.File
{
    public class AddFileCommandHandler:IRequestHandler<AddFileCommand,Result<string>>
    {
        private readonly IFileServices<StoreRelease, int> _fileServices;

        public AddFileCommandHandler(IFileServices<StoreRelease, int> fileServices)
        {
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
