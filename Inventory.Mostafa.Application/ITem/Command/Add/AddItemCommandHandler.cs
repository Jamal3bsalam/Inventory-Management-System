using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Command.Add
{
    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, Result<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ItemDto>> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ItemsName) || request.StockNumber == 0) return Result<ItemDto>.Failure("Please fill all required fields correctly.");
            var item = new Items()
            {
                ItemsName = request.ItemsName,
                StockNumber = request.StockNumber,
            };

            await _unitOfWork.Repository<Items, int>().AddAsync(item);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<ItemDto>.Failure("Faild To Add Item.");

            var itemDto = item.Adapt<ItemDto>();

            return Result<ItemDto>.Success(itemDto, "Item Added Successfully");
        }
    }
}
