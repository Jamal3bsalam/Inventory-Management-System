using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Units.Command.Add
{
    public class AddUnitCommandHandler : IRequestHandler<AddUnitCommand, Result<UnitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUnitCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<UnitDto>> Handle(AddUnitCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UnitName) || request.Recipients.Count() == 0) return Result<UnitDto>.Failure("Please fill all required fields correctly.");

            var unit = new Unit()
            {
                UnitName = request.UnitName,
                Recipients = request.Recipients.Select(R => new Recipients() { Name = R.Name }).ToList()
            };

            await _unitOfWork.Repository<Unit,int>().AddAsync(unit);
            var result = await _unitOfWork.CompleteAsync();

            var unitDto = unit.Adapt<UnitDto>();

            if(result <= 0) return Result<UnitDto>.Failure("Faild To Add Unit.");

            return Result<UnitDto>.Success(unitDto, "Unit Added Successfully");
        }
    }
}
