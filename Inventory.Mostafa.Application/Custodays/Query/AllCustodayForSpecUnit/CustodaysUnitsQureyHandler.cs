using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Custodays.Query.AllCustodayForSpecUnit
{
    public class CustodaysUnitsQureyHandler : IRequestHandler<CustodaysUnitsQurey, Result<IEnumerable<CustodaysUnitsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustodaysUnitsQureyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<CustodaysUnitsDto>>> Handle(CustodaysUnitsQurey request, CancellationToken cancellationToken)
        {
            if (request.UnitId == null || request.UnitId == 0) return Result<IEnumerable<CustodaysUnitsDto>>.Failure("Please Enter A Valid Id");

            var custodaySpec = new CustodaySpec(request.UnitId.Value,true);
            var custodays = await _unitOfWork.Repository<Custoday,int>().GetAllWithSpecAsync(custodaySpec);

            if (custodays == null) return Result<IEnumerable<CustodaysUnitsDto>>.Failure("Faild To Retrive All Custodays");

            var custodayDto = custodays.Select(c => new CustodaysUnitsDto()
            {
                CustodayId = c.Id,
                CurrentRecipintsId = c.RecipientsId,
                CurrentRecipints = c.Recipients.Name,
                CustodaysUnitsItems = c.CustodyItems.Where(i => i.IsDeleted == false).Select(i => new CustodaysUnitsItemsDto()
                {
                    ItemId = i.Item.Id,
                    ItemName = i.Item.ItemsName,
                    Quantity = i.Quantity
                }).ToList()
            });

            return Result<IEnumerable<CustodaysUnitsDto>>.Success(custodayDto, "Custodays Retrived Successfully.");
        }
    }
}
