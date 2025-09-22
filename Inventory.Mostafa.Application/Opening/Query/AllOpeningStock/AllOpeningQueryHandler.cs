using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Mostafa.Domain.Specification.OpeningSpecification;
using Inventory.Mostafa.Domain.Entities.Opening;

namespace Inventory.Mostafa.Application.Opening.Query.AllOpeningStock
{
    public class AllOpeningQueryHandler : IRequestHandler<AllOpeningQuery, Result<Pagination<IEnumerable<OpeningDtos>>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllOpeningQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Pagination<IEnumerable<OpeningDtos>>>> Handle(AllOpeningQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<SpecParameter>();
            var spec = new OpeningSpec(parameter);
            var count = new OpeningCountSpec(parameter);
            var openingStocks = await _unitOfWork.Repository<OpeningStock, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<OpeningStock, int>().GetCountAsync(count);
            if (openingStocks == null) return Result<Pagination<IEnumerable<OpeningDtos>>>.Failure("Faild To Retrive All Opening Stocks");

            var openingDtos = openingStocks.Select(o => new OpeningDtos()
            {
                Id = o.Id,
                ItemName = o.Items?.ItemsName,
                StockNumber = o.Items?.StockNumber,
                Quantity = o.Quantity,
            });

            var pagintion = new Pagination<IEnumerable<OpeningDtos>>(parameter.pageSize, parameter.pageIndex, counts, openingDtos);

            return Result<Pagination<IEnumerable<OpeningDtos>>>.Success(pagintion, "All Opening Stocks Retrived Successfully.");
        }
    }
}
