using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
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

namespace Inventory.Mostafa.Application.Custodays.Query.AllTransfers
{
    public class AllTransfersQueryHandler : IRequestHandler<AllTransfersQuery, Result<Pagination<IEnumerable<TransactionDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllTransfersQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<TransactionDto>>>> Handle(AllTransfersQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseSpecParameter>();
            var spec = request.UnitId == null ? new TransferSpec(parameter) : new TransferSpec(parameter, true);
            var count = request.UnitId == null ? new TransferCount(parameter) : new TransferCount(parameter, true);

            var transfers = await _unitOfWork.Repository<CustodayTransfers, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<CustodayTransfers, int>().GetCountAsync(count);



            if (transfers == null) return Result<Pagination<IEnumerable<TransactionDto>>>.Failure("Faild To Retrived All Transfers.");

            var returnsDto = transfers.Select(r => new TransactionDto()
            {
                Id = r.Id,
                UnitName = r.Unit?.UnitName,
                OldRecipints = r.OldRecipient?.Name,
                NewRecipints = r.NewRecipient?.Name,
                ItemName = r.Item?.ItemsName,
                Quantity = r.Quantity,
                TransactionDate = r.TransactionDate,
                DocumentPath = r.DocumentPath != null ? _configuration["BASEURL"] + r.DocumentPath : null,                
            });
            var pagintion = new Pagination<IEnumerable<TransactionDto>>(parameter.PageSize, parameter.PageIndex, counts, returnsDto);

            return Result<Pagination<IEnumerable<TransactionDto>>>.Success(pagintion, "All Transfers Retrived Successfully.");
        }
    }
}
