using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Units.Query.AllRecipintsForUnit
{
    public class UnitRecipintsQueryHandler : IRequestHandler<UnitRecipintsQuery, Result<IEnumerable<RecipintsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitRecipintsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<RecipintsDto>>> Handle(UnitRecipintsQuery request, CancellationToken cancellationToken)
        {
            if (request.UnitId == null || request.UnitId == 0) return Result<IEnumerable<RecipintsDto>>.Failure("Please Enter Valid Id");
            var recipintsSpec = new RecipintsSpec(request.UnitId.Value, true);
            var recipints = await _unitOfWork.Repository<Recipients,int>().GetAllWithSpecAsync(recipintsSpec);

            if (recipints == null) return Result<IEnumerable<RecipintsDto>>.Failure($"No Recipints For Unit With This Id: {request.UnitId}");

            var recipintsDto = recipints.Select(r => new RecipintsDto()
            {
                Id = r.Id,
                Name = r.Name,  
            });

            return Result<IEnumerable<RecipintsDto>>.Success(recipintsDto, "Recipints Retrived Successfully");
        }
    }
}
