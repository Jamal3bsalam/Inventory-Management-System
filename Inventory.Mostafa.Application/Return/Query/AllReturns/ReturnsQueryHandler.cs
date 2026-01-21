using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
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
    public class ReturnsQueryHandler : IRequestHandler<ReturnsQuery, Result<Pagination<IEnumerable<AllReturnDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ReturnsQueryHandler(IUnitOfWork unitOfWork , IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<AllReturnDto>>>> Handle(ReturnsQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<StoreReleaseSpecParameter>();
            var spec = request.UnitId == null ? new ReturnSpec(parameter) : new ReturnSpec(parameter, true);
            var count = request.UnitId == null ? new ReturnCount(parameter) : new ReturnCount(parameter, true);

            var returns = await _unitOfWork.Repository<Returns, int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Returns, int>().GetCountAsync(count);

            

            if (returns == null) return Result<Pagination<IEnumerable<AllReturnDto>>>.Failure("Faild To Retrived All Returns.");


            var returnsDto = returns.Select(r => new AllReturnDto()
            {
                Id = r.Id,
                UnitName = r.Unit?.UnitName,
                RecipintsName = r.Recipients?.Name,
                ReturnDocUrl = string.IsNullOrEmpty(r.DocumentPath) ? null : _configuration["BASEURL"] + r.DocumentPath,
                Reason = r.Reason,
                ReturnItems = r.ReturnItems?.Select(ri => new ReturnItemResponseDto()
                {
                    ExpenseId = ri.UnitExpenseId,
                    DocumentNumber = ri.UnitExpense?.DocumentNumber,
                    DocumentUrl = string.IsNullOrEmpty(ri.UnitExpense?.AttachmentUrl) ? null : _configuration["BASEURL"] + ri.UnitExpense?.AttachmentUrl,
                    ItemName = ri.Item?.ItemsName,
                    Quantity = ri.Quantity
                }).ToList()
            });
            var pagintion = new Pagination<IEnumerable<AllReturnDto>>(parameter.PageSize, parameter.PageIndex, counts, returnsDto);

            return Result<Pagination<IEnumerable<AllReturnDto>>>.Success(pagintion, "All Store Release Retrived Successfully.");

        }
    }
}
