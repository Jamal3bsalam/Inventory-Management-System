using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.ITem.Query.AllItems;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Inventory.Mostafa.Domain.Specification;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using Inventory.Mostafa.Domain.Entities.Identity;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Units.Query.AllUnits
{
    public class AllUnitQueryHandler : IRequestHandler<AllUnitsQuery, Pagination<IEnumerable<UnitDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllUnitQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Pagination<IEnumerable<UnitDto>>> Handle(AllUnitsQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<SpecParameter>();
            var spec = new UnitSpec(parameter);
            var count = new UnitCountSpec(parameter);
            var units = await _unitOfWork.Repository<Unit,int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Unit, int>().GetCountAsync(count);

            if (units == null) return null;
            var unitsDto = units.Adapt<IEnumerable<UnitDto>>();

            return new Pagination<IEnumerable<UnitDto>>(request.pageSize, request.pageIndex, counts, unitsDto);
        }
    }
}
