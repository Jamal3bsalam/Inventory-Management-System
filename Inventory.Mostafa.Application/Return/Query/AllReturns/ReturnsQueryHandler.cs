using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Return.Query.AllReturns
{
    public class ReturnsQueryHandler : IRequestHandler<ReturnsQuery, Result<Pagination<IEnumerable<ReturnDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ReturnsQueryHandler(IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<ReturnDto>>>> Handle(ReturnsQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseSpecParameter>();
            var spec = request.UnitId == null ? new ReturnSpec(parameter) : new ReturnSpec(parameter, true);
            var count = request.UnitId == null ? new ReturnCount(parameter) : new ReturnCount(parameter, true);

            var returns = await _unitOfWork.Repository<Returns, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Returns, int>().GetCountAsync(count);



            if (returns == null) return Result<Pagination<IEnumerable<ReturnDto>>>.Failure("Faild To Retrived All Returns.");

            var returnsDto = returns.Select(r => new ReturnDto()
            {
                Id = r.Id,
                UnitName = r.Unit?.UnitName,
                RecipintsName = r.Recipients?.Name,
                ItemName = r.StoreReleaseItem.OrderItem.ItemName,
                DocumentUrl = _configuration["BASEURL"] + r.DocumentPath,
                Quantity = r.Quantity,
                Reason = r.Reason,
            });
            var pagintion = new Pagination<IEnumerable<ReturnDto>>(parameter.PageSize, parameter.PageIndex, counts, returnsDto);

            return Result<Pagination<IEnumerable<ReturnDto>>>.Success(pagintion, "All Store Release Retrived Successfully.");

        }
    }
}
