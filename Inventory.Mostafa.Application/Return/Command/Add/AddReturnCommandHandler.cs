using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;

namespace Inventory.Mostafa.Application.Return.Command.Add
{
    public class AddReturnCommandHandler : IRequestHandler<AddReturnCommand, Result<ReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<Returns, int> _fileServices;

        public AddReturnCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration,IFileServices<Returns,int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<ReturnDto>> Handle(AddReturnCommand request, CancellationToken cancellationToken)
        {

            var returns = new Returns();

            var unitSpec = new UnitSpec(request.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit,int>().GetWithSpecAsync(unitSpec);
            if (unit == null) return Result<ReturnDto>.Failure($"Unit With Id: {request.UnitId} Not Fount");

            var recipintsSpec = new RecipintsSpec(request.RecipintsId.Value);
            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);
            if (recipints == null) return Result<ReturnDto>.Failure($"Recipints With Id: {request.RecipintsId} Not Fount");

            var custodaySpec = new CustodaySpec(request.RecipintsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

            var storeItemsSpec = new StoreItemSpec(request.StoreReleaseItemId.Value, true);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetWithSpecAsync(storeItemsSpec);
            if (storeItem == null) return Result<ReturnDto>.Failure($"Store Item With Id: {request.StoreReleaseItemId} Not Fount");

            if(request.Quantity > storeItem.Quantity) return Result<ReturnDto>.Failure($"Return quantity cannot be greater than the released quantity(Available: {storeItem.Quantity}, Requested: {request.Quantity}).");
            if (request.Document != null)
            {
                string fileName = _fileServices.Upload(request.Document);
                returns.DocumentPath = $"\\Files\\Returns\\{fileName}";
            }
            else
            {
                returns.DocumentPath = null;
            }
            returns.UnitId = request.UnitId;
            returns.RecipientsId = request.RecipintsId;
            returns.storeReleaseItemId = request.StoreReleaseItemId;
            returns.Quantity = request.Quantity;
            returns.Reason = request.Reason;

            await _unitOfWork.Repository<Returns,int>().AddAsync(returns);

            var custodayItems = custoday?.CustodyItems?.FirstOrDefault(c => c.CustodyId == custoday.Id && c.ItemId == storeItem.ItemId);
            if (custodayItems != null)
            {
                custodayItems.Quantity -= request.Quantity;
                _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
            }
            storeItem.OrderItem.ConsumedQuantity -= request.Quantity;
            _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<ReturnDto>.Failure("Faild To Add Returns");

            var returnDto = new ReturnDto()
            {
                Id = returns.Id,
                UnitName = unit.UnitName,
                RecipintsName = recipints.Name,
                ItemName = storeItem.OrderItem.ItemName,
                DocumentUrl = returns.DocumentPath != null ? _configuration["BASEURL"] + returns.DocumentPath : null,
                Quantity = returns.Quantity,
                Reason = returns.Reason,
            };

            return Result<ReturnDto>.Success(returnDto,"Returns Add Successfully");    
        }
    }
}
