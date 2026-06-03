using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Inventory.Mostafa.Application.Store.Query.AllForSpecItem
{
    public class AllStoreForItemQueryHandler : IRequestHandler<AllStoreForItemQuery, Result<Pagination<IEnumerable<StoreReleaseDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllStoreForItemQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<StoreReleaseDto>>>> Handle(AllStoreForItemQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseItemParameters>();
            var spec = request.UnitId == null ? new StoreReleaseSpec(parameter) : new StoreReleaseSpec(parameter);
            var count = request.UnitId == null ? new StoreReleaseCountSpec(parameter) : new StoreReleaseCountSpec(parameter);

            var storeReleases = await _unitOfWork.Repository<StoreRelease, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<StoreRelease, int>().GetCountAsync(count);


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
                    ItemId = i.Items?.Id,
                    OrderId = i.OrderId,
                    OrderNumber = i.Order?.OrderNumber,
                    OrderType = i.Order?.OrderType,
                    OrderItemId = i.OrderItemId,
                    Quantity = i.Quantity,
                    SerialNumbers = i.SerialNumbers?.Select(sn => sn.SerialNumber).ToList()
                }).Where(i => i.ItemId == parameter.ItemId).ToList()
            }).Where(i => i.Items.Any());
            var pagintion = new Pagination<IEnumerable<StoreReleaseDto>>(parameter.PageSize, parameter.PageIndex, counts, storeReleasesDto);

            return Result<Pagination<IEnumerable<StoreReleaseDto>>>.Success(pagintion, "All Store Release Retrived Successfully.");
        }
    }
}
