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
using Inventory.Mostafa.Domain.Specification.Return;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Application.Return.Command.Update
{
    internal class UpdateReturnCommandHandler : IRequestHandler<UpdateReturnCommand, Result<ReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileServices<Returns, int> _fileServices;

        public UpdateReturnCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration, IFileServices<Returns, int> fileServices)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileServices = fileServices;
        }
        public async Task<Result<ReturnDto>> Handle(UpdateReturnCommand request, CancellationToken cancellationToken)
        {
            if (request.ReturnId == 0 || request.ReturnId == null) return Result<ReturnDto>.Failure("Please Enter Valid Id.");
            var returnSpec = new ReturnSpec(request.ReturnId.Value);
            var returns = await _unitOfWork.Repository<Returns,int>().GetWithSpecAsync(returnSpec);
            if (returns == null) return Result<ReturnDto>.Failure($"Return With This Id: {request.ReturnId} Not Found");

            var recipintsSpec = new RecipintsSpec(returns.RecipientsId.Value);
            var recipints = await _unitOfWork.Repository<Recipients, int>().GetWithSpecAsync(recipintsSpec);

            var unitSpec = new UnitSpec(returns.UnitId.Value);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(unitSpec);

            var custodaySpec = new CustodaySpec(returns.RecipientsId.Value);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);

            var storeItemsSpec = new StoreItemSpec(returns.storeReleaseItemId.Value, true);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetWithSpecAsync(storeItemsSpec);

            if (request.Quantity > storeItem.OrderItem.Quantity) return Result<ReturnDto>.Failure($"Return quantity cannot be greater than the released quantity(Available: {storeItem.Quantity}, Requested: {request.Quantity}).");


            if (!string.IsNullOrEmpty(request.Reason))
                returns.Reason = request.Reason;

            if (request.File != null && !string.IsNullOrEmpty(request.File.FileName))
            {
                if (!string.IsNullOrEmpty(returns.DocumentPath))
                {
                    _fileServices.Delete(returns.DocumentPath);
                }

                var fileName = _fileServices.Upload(request.File);
                returns.DocumentPath = $"\\Files\\Returns\\{fileName}";
            }


            if (request.Quantity.HasValue && request.Quantity > 0)
            {

                var old = returns.Quantity;
                var newQ = request.Quantity;

                var diff = newQ - old;
                if (diff != 0)
                {
                    returns.Quantity = newQ;
                    var custodayItems = custoday?.CustodyItems?.FirstOrDefault(c => c.CustodyId == custoday.Id && c.ItemId == storeItem.ItemId);
                    if (custodayItems != null)
                    {
                        custodayItems.Quantity -= diff;
                        if (custodayItems.Quantity < 0)
                            return Result<ReturnDto>.Failure("Custody quantity cannot be negative");

                        _unitOfWork.Repository<CustodyItem, int>().Update(custodayItems);
                    }
                    storeItem.OrderItem.ConsumedQuantity -= diff;
                    if (storeItem.OrderItem.ConsumedQuantity < 0)
                        return Result<ReturnDto>.Failure("Consumed quantity cannot be negative");
                    if (storeItem.OrderItem.ConsumedQuantity > storeItem.OrderItem.Quantity)
                        return Result<ReturnDto>.Failure("Consumed quantity cannot exceed ordered quantity");
                    _unitOfWork.Repository<StoreReleaseItem, int>().Update(storeItem);
                }

            }

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return Result<ReturnDto>.Failure("Faild To Update Returns");

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

            return Result<ReturnDto>.Success(returnDto, "Returns Updated Successfully");


        }
    }
}
