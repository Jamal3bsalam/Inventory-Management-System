using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Query.RecipintsCustoday
{
    public class AllRecipintsCustodayQueryHandler : IRequestHandler<AllRecipintsCustodayQuery, Result<IEnumerable<RecipintsCustodayDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllRecipintsCustodayQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<RecipintsCustodayDto>>> Handle(AllRecipintsCustodayQuery request, CancellationToken cancellationToken)
        {
            //if (request.UnitId == 0 || request.UnitId == null || request.RecipintsId == 0 || request.RecipintsId == null) return Result<IEnumerable<RecipintsCustodayDto>>.Failure("Please Enter A Valid Data.");


            Custoday custoday = new();

            if (request.UnitId != null && request.UnitId != 0)
            {
                var unitCustodaySpec = new CustodaySpec(request.UnitId, true);
                custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(unitCustodaySpec);
                if (custoday == null) return Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custoday For This Unit.");
            }
            else
            {
                var recipintsCustodaySpec = new CustodaySpec(request.RecipintsId);
                custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(recipintsCustodaySpec);
                if (custoday == null) Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custoday For This Recipints.");
            }   

            var returnSpec = new ReturnSpec(request.RecipintsId,true,true);
            var recipintsReturns = await _unitOfWork.Repository<Returns, int>().GetAllWithSpecAsync(returnSpec);

            var recipintsCustodayDto = custoday.CustodyItems.Select(i =>
            {
                // Filter all returns related to this item
                var itemReturns = recipintsReturns?.Where(r => r.ItemId == i.ItemId).ToList();

                var totalReturned = itemReturns?.Sum(r => r.Quantity ?? 0) ?? 0;
                var totalWriteOff = itemReturns?.Sum(r => r.WriteOfQuantity) ?? 0;

                return new RecipintsCustodayDto
                {
                    ItemName = i.Item?.ItemsName,
                    OriginalQuantity = i.Quantity + totalReturned + totalWriteOff,
                    ReturnedQuantity = totalReturned + totalWriteOff,
                    RemainingQuantity = i.Quantity
                };
            });

            return Result<IEnumerable<RecipintsCustodayDto>>.Success(recipintsCustodayDto, "All Recipints Custoday Retrived Successfully");

        }
    }
}
