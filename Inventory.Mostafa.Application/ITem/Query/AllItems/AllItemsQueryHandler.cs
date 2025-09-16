using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Mapster;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.ITem.Query.AllItems
{
    public class AllItemsQueryHandler : IRequestHandler<AllItemsQuery, Pagination<IEnumerable<ItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllItemsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Pagination<IEnumerable<ItemDto>>> Handle(AllItemsQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<SpecParameter>();
            var spec = new ItemSpec(parameter);
            var count = new ItemCountSpecifications(parameter);
            var items = await _unitOfWork.Repository<Items, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Items, int>().GetCountAsync(count);

            if (items == null) return null;
            var itemsDto = items.Adapt<IEnumerable<ItemDto>>();

            return new Pagination<IEnumerable<ItemDto>>(request.pageSize, request.pageIndex, counts,itemsDto);
            
        }
    }
}
