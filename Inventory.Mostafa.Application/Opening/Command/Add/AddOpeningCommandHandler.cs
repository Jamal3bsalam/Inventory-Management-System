using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Inventory.Mostafa.Domain.Specification.OpeningSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Add
{
    public class AddOpeningCommandHandler : IRequestHandler<AddOpeningCommand, Result<OpeningDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddOpeningCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OpeningDto>> Handle(AddOpeningCommand request, CancellationToken cancellationToken)
        {
            if (request.ItemId == null || request.Quantity == null) return Result<OpeningDto>.Failure("Fill The Fields With The Right Data");

            var spec = new ItemSpec(request.ItemId.Value);
            var item = await _unitOfWork.Repository<Items,int>().GetWithSpecAsync(spec);
            if (item == null)
                return Result<OpeningDto>.Failure("Item not found");
            var openingStock = new OpeningStock()
            {
                ItemsId = request.ItemId.Value,
                Quantity = request.Quantity.Value,
                CreatedAt = DateTime.UtcNow,
                SerialNumbers = request.SerialNumbers.Select(s => new OpeningSerialNumber()
                {
                    SerialNumber = s,
                }).ToList()
            };

           await _unitOfWork.Repository<OpeningStock,int>().AddAsync(openingStock);
          var result =  await _unitOfWork.CompleteAsync();

            var openingDto = new OpeningDto()
            {
                Id = openingStock.Id,
                ItemName = item.ItemsName,
                StockNumber = item.StockNumber,
                Quantity = request.Quantity.Value,
                SerialNumbers = openingStock.SerialNumbers.Select(s => s.SerialNumber).ToList()
            };

            if (result <= 0) return Result<OpeningDto>.Failure("Faild To Add Opening Stock.");

            return Result<OpeningDto>.Success(openingDto, "Opening Stock Added Successfully.");
        }
    }
}
