using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Return;
using MediatR;
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


            List<Custoday> custodays = new List<Custoday>();

            if (request.UnitId != null && request.UnitId != 0)
            {
                var unitCustodaySpec = new CustodaySpec(request.UnitId, true);
                var custodaysDb = await _unitOfWork.Repository<Custoday, int>().GetAllWithSpecAsync(unitCustodaySpec);

                custodays.AddRange(custodaysDb);

                if (custodays == null)
                    return Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custodys For This Unit.");
            }
            else
            {
                var recipintsCustodaySpec = new CustodaySpec(request.RecipintsId);
                var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(recipintsCustodaySpec);

                if (custoday == null)
                    return Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custody For This Recipient.");

                custodays.Add(custoday);
            }

            // 🔹 Get all returns WITH ReturnItems
            List<Returns> recipintsReturnsDetails = new List<Returns>();
            if (request.UnitId != null && request.UnitId != 0)
            {
                var returnSpec = new ReturnSpec(request.UnitId, true);
                var recipintsReturns = await _unitOfWork.Repository<Returns, int>().GetAllWithSpecAsync(returnSpec);
                recipintsReturnsDetails = (List<Returns>)recipintsReturns;
            }
            else
            {
                var returnSpec = new ReturnSpec(request.RecipintsId, true, true);
                var recipintsReturns = await _unitOfWork.Repository<Returns, int>().GetAllWithSpecAsync(returnSpec);
                recipintsReturnsDetails = (List<Returns>)recipintsReturns;
            }

            // Flatten ReturnItems
            var allReturnItems = recipintsReturnsDetails
                .SelectMany(r => r.ReturnItems ?? Enumerable.Empty<ReturnItem>())
                .ToList();


            var resultDto = new List<RecipintsCustodayDto>();

            foreach (var custody in custodays)
            {
                if (request.RecipintsId != null && request.RecipintsId != 0)
                {
                    if (custody.CustodyItems == null || !custody.CustodyItems.Any())
                        return Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custody Items For This Recipient.");
                }

                if (request.UnitId != null && request.UnitId != 0)
                {
                    if (custody.CustodyItems == null || !custody.CustodyItems.Any())
                        return Result<IEnumerable<RecipintsCustodayDto>>.Failure("There Is No Custody Items For This Unit.");
                }
                    foreach (var custodyItem in custody.CustodyItems.Where(ci => ci.IsDeleted == false))
                    {
                      // 🔹 Returned Quantity
                      var returnedQuantity = allReturnItems
                          .Where(ri => ri.ItemId == custodyItem.ItemId &&
                                 ri.Return?.RecipientsId == custody.RecipientsId)
                          .Sum(ri => ri.Quantity);
                      // 🔹 Original Quantity
                      var originalQuantity = custodyItem.Quantity + returnedQuantity;
                      resultDto.Add(new RecipintsCustodayDto
                      {
                          ItemName = custodyItem.Item?.ItemsName,
                          OriginalQuantity = originalQuantity,
                          ReturnedQuantity = returnedQuantity,
                          RemainingQuantity = custodyItem.Quantity
                      });
                    }
                
            }

            return Result<IEnumerable<RecipintsCustodayDto>>
                    .Success(resultDto, "All Recipients Custody Retrieved Successfully");
        }
        
    }
}
