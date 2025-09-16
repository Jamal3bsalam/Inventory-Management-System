using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Command.Update
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ItemDto>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ItemsName) || request.StockNumber == 0) return Result<ItemDto>.Failure("Please fill all required fields correctly.");

            var spec = new ItemSpec(request.Id.Value);
            var item = await _unitOfWork.Repository<Items,int>().GetWithSpecAsync(spec);

            if (item == null) return Result<ItemDto>.Failure("There Is No Item With This Id.");

            item.ItemsName = request.ItemsName;
            item.StockNumber = request.StockNumber;

             _unitOfWork.Repository<Items,int>().Update(item);
            var result = await _unitOfWork.CompleteAsync();

            if(result <= 0) Result<ItemDto>.Failure("Faild To Update This Item.");

            var itemDto = item.Adapt<ItemDto>();

            return Result<ItemDto>.Success(itemDto,"Item Updated Successfully.");
        }
    }
}
