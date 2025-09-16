using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OpeningSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Opening.Command.Delete
{
    public class UpdateOpeningCommandHandler : IRequestHandler<UpdateOpeningCommand, Result<OpeningDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOpeningCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OpeningDto>> Handle(UpdateOpeningCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<OpeningDto>.Failure("Please Enter A Valid Id.");

            var spec = new OpeningSpec(request.Id.Value);
            var openingStock = await _unitOfWork.Repository<OpeningStock,int>().GetWithSpecAsync(spec);
            if (openingStock == null) return Result<OpeningDto>.Failure("Opening stock not found.");

            openingStock.Quantity += request.NewQuantity;
            if (request.NewSerialNumbers?.Any() == true)
            {
                foreach (var serial in request.NewSerialNumbers)
                {
                    openingStock.SerialNumbers.Add(new OpeningSerialNumber
                    {
                        OpeningStockId = openingStock.Id,
                        SerialNumber = serial
                    });
                }
            }

            var openingDto = new OpeningDto()
            {
                Id = openingStock.Id,
                ItemName = openingStock.Items?.ItemsName,
                StockNumber = openingStock.Items?.StockNumber,
                Quantity = openingStock.Quantity,
                SerialNumbers = openingStock.SerialNumbers.Select(s => s.SerialNumber).ToList()
            };

            _unitOfWork.Repository<OpeningStock,int>().Update(openingStock);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) return Result<OpeningDto>.Failure("Faild To Update Opening Stock.");

            return Result<OpeningDto>.Success(openingDto, "Opening Stock Updated Successfully.");

        }
    }
}
