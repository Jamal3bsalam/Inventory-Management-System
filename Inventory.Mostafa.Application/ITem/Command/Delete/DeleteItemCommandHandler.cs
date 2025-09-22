using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Command.Delete
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0) return Result<string>.Failure("Please fill all required fields correctly.");

            var spec = new ItemSpec(request.Id.Value);
            var item = await _unitOfWork.Repository<Items, int>().GetWithSpecAsync(spec);

            if (item == null) return Result<string>.Failure("There Is No Item With This Id.");
            item.IsDeleted = true;
            item.DeletedAt = DateTime.UtcNow;   
            _unitOfWork.Repository<Items,int>().Update(item);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) Result<ItemDto>.Failure("Faild To Delete This Item.");

            return Result<string>.Success("Item Deleted Successfully.");
        }
    }
}
