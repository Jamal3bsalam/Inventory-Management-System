using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Store.Query.AllStoreRelease
{
    public class StoreReleasesQueryHandler : IRequestHandler<StoreReleasesQuery, Result<Pagination<IEnumerable<StoreReleaseDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public StoreReleasesQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<StoreReleaseDto>>>> Handle(StoreReleasesQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseSpecParameter>();
            var spec = request.UnitId == null ? new StoreReleaseSpec(parameter) : new StoreReleaseSpec(parameter, true);
            var count = request.UnitId == null ? new StoreReleaseCountSpec(parameter) : new StoreReleaseCountSpec(parameter, true);

            var storeReleases = await _unitOfWork.Repository<StoreRelease, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<StoreRelease, int>().GetCountAsync(count);



            if (storeReleases == null) return Result<Pagination<IEnumerable<StoreReleaseDto>>>.Failure("Faild To Retrived All Store Releases.");

            var storeReleasesDto = storeReleases.Select(S => new StoreReleaseDto()
            {
                Id = S.Id,
                UnitName = S.Unit?.UnitName,
                ReceiverName = S.Recipients?.Name,
                ReleaseDate = S.ReleaseDate,
                DocumentNumber = S.DocumentNumber,
                StoreReleaseType = "صرف من المستودع",
                FileUrl = _configuration["BASEURL"] + S.DocumentPath,
                Items = S.StoreReleaseItems.Select(i => new StoreReleaseItemResponseDto
                {
                    Id = i.Id,
                    ItemName = i.Items?.ItemsName,
                    OrderId = i.OrderId,
                    OrderNumber = i.Order?.OrderNumber,
                    OrderType = i.Order?.OrderType,
                    OrderItemId = i.OrderItemId,
                    Quantity = i.Quantity,
                    SerialNumbers = i.SerialNumbers?.Select(sn => sn.SerialNumber).ToList()
                }).ToList()
            });
            var pagintion = new Pagination<IEnumerable<StoreReleaseDto>>(parameter.PageSize, parameter.PageIndex, counts, storeReleasesDto);

            return Result<Pagination<IEnumerable<StoreReleaseDto>>>.Success(pagintion,"All Store Release Retrived Successfully.");
        }
    }
}
