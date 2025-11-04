using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
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

namespace Inventory.Mostafa.Application.Write.Query.AllWriteOff
{
    public class WriteOffsQueryHandler : IRequestHandler<WriteOffsQuery, Result<Pagination<IEnumerable<WriteOffDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public WriteOffsQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<WriteOffDto>>>> Handle(WriteOffsQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseSpecParameter>();
            var spec = request.UnitId == null ? new WriteOffSpec(parameter) : new WriteOffSpec(parameter, true);
            var count = request.UnitId == null ? new WriteOffCount(parameter) : new WriteOffCount(parameter, true);

            var writeOffs = await _unitOfWork.Repository<WriteOff, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<WriteOff, int>().GetCountAsync(count);



            if (writeOffs == null) return Result<Pagination<IEnumerable<WriteOffDto>>>.Failure("Faild To Retrived All WriteOffs.");

            var writeOffsDto = writeOffs.Select(r => new WriteOffDto()
            {
                Id = r.Id,
                UnitName = r.Unit?.UnitName,
                RecipintsName = r.Recipients?.Name,
                ItemName = r.Returns?.Item?.ItemsName,
                DocumetPath = r.DocumentPath != null ? _configuration["BASEURL"] + r.DocumentPath : null,
                Quantity = r.Quantity,
            }) ;
            var pagintion = new Pagination<IEnumerable<WriteOffDto>>(parameter.PageSize, parameter.PageIndex, counts, writeOffsDto);

            return Result<Pagination<IEnumerable<WriteOffDto>>>.Success(pagintion, "All WriteOffs Retrived Successfully.");
        }
    }
}
