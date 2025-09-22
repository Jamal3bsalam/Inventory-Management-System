using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Units.Command.Update
{
    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, Result<UnitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUnitCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<UnitDto>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0) return Result<UnitDto>.Failure("Please fill all required fields correctly.");
            var spec = new UnitSpec(request.Id.Value);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(spec);

            if (unit == null) return Result<UnitDto>.Failure("There Is No Unit With This Id.");

            if (!string.IsNullOrEmpty(request.UnitName))
            {
                unit.UnitName = request.UnitName;
            }


            var existRecipints = unit.Recipients.ToList();

            foreach(var recipient in request.Recipients)
            {
                if (recipient.Id != 0)
                {
                    var existing = existRecipints.FirstOrDefault(r => r.Id == recipient.Id);
                    if (existing != null)
                    {
                        existing.Name = recipient.Name;
                        existRecipints.Remove(existing); 
                    }
                }
                else
                {
                    // ✅ Insert
                    unit.Recipients.Add(new Recipients
                    {
                        Name = recipient.Name,
                        UnitId = unit.Id
                    });
                }
            }
            //foreach (var toDelete in existRecipints)
            //{
            //    unit.Recipients.Remove(toDelete);
            //}


            _unitOfWork.Repository<Unit, int>().Update(unit);
            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0) Result<UnitDto>.Failure("Faild To Update This Unit.");

            var unitDto = unit.Adapt<UnitDto>();

            return Result<UnitDto>.Success(unitDto, "Unit Updated Successfully.");
        }
    }
}
