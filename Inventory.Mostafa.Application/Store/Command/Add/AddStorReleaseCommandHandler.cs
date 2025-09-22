using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
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

namespace Inventory.Mostafa.Application.Store.Command.Add
{
    public class AddStorReleaseCommandHandler : IRequestHandler<AddStoreReleaseCommand, Result<StoreReleaseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AddStorReleaseCommandHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<StoreReleaseDto>> Handle(AddStoreReleaseCommand request, CancellationToken cancellationToken)
        {
            var uSpec = new UnitSpec(request.UnitId);
            var unit = await _unitOfWork.Repository<Unit, int>().GetWithSpecAsync(uSpec);
            if (unit == null)
                return Result<StoreReleaseDto>.Failure($"Unit with Id {request.UnitId} not found");
            var recipints = unit.Recipients.FirstOrDefault(R => R.Id == request.ReceiverId);
            if (recipints == null) return Result<StoreReleaseDto>.Failure($"Recipint With This Id: {request.ReceiverId} not found");

            foreach (var item in request.Items)
            {
                var oSpec = new OrderSpec(item.OrderId);
                var order = await _unitOfWork.Repository<Orders, int>().GetWithSpecAsync(oSpec);
                if (order == null)
                    return Result<StoreReleaseDto>.Failure($"Order With This Id: {item.OrderId} not found");

                var orderItem = order.OrderItems?.FirstOrDefault(OI => OI.Id == item.OrderItemId);
                if (orderItem == null)
                    return Result<StoreReleaseDto>.Failure($"Order Item With This Id: {item.OrderItemId} not found");

                if (item.Quantity > (orderItem.Quantity - orderItem.ConsumedQuantity))
                    return Result<StoreReleaseDto>.Failure($"Quantity more than available stock for Order Item {item.OrderItemId}");
            }

            var storeRelease = new StoreRelease()
            {
                UnitId = request.UnitId,
                RecipientsId = request.ReceiverId,
                DocumentNumber = request.DocumentNumber,
                DocumentPath = $"\\Files\\StoreRelease\\{request.DocumentPath}",
                ReleaseDate = DateTime.UtcNow,
                StoreReleaseItems = new List<StoreReleaseItem>()
            };

            await _unitOfWork.Repository<StoreRelease, int>().AddAsync(storeRelease);
            var saved = await _unitOfWork.CompleteAsync() > 0;
            if (!saved) return Result<StoreReleaseDto>.Failure("Failed to save store release");

            var spec = new CustodaySpec(request.ReceiverId);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(spec);

            if(custoday == null)
            {
                custoday = new Custoday()
                {
                    RecipientsId = request.ReceiverId,
                    UnitId = request.UnitId,
                    CreatedDate = DateTime.UtcNow,
                    CustodyItems = new List<CustodyItem>()
                };
                await _unitOfWork.Repository<Custoday, int>().AddAsync(custoday);
                await _unitOfWork.CompleteAsync();
            }
            else if (custoday.CustodyItems == null)
            {
                custoday.CustodyItems = new List<CustodyItem>();
            }

            foreach (var item in request.Items)
            {
                var oSpec = new OrderSpec(item.OrderId);
                var order = await _unitOfWork.Repository<Orders, int>().GetWithSpecAsync(oSpec);
                var orderItems = order?.OrderItems?.First(OI => OI.Id == item.OrderItemId);

                var storeReleaseItem = new StoreReleaseItem()
                    {
                        OrderId = order.Id,
                        OrderItemId = orderItems.Id,
                        ItemId = orderItems.ItemsId,
                        StoreReleaseId = storeRelease.Id,
                        Quantity = item.Quantity,
                        SerialNumbers = item.SerialNumbers.Select(s => new ReleaseItemSerialNumber() { SerialNumber = s }).ToList()
                    };
                await _unitOfWork.Repository<StoreReleaseItem, int>().AddAsync(storeReleaseItem);

                orderItems.ConsumedQuantity += item.Quantity;
                _unitOfWork.Repository<OrderItems, int>().Update(orderItems);

                var stockTransaction = new StockTransaction()
                {
                    ItemId = orderItems.ItemsId,
                    OrderItemsId = orderItems.Id,
                    TransactionType = "Release",
                    TransactionDate = DateTime.UtcNow,
                    Quantity = -item.Quantity,
                    BalanceAfter = orderItems.Quantity - orderItems.ConsumedQuantity,
                    RelatedId = storeRelease.Id,
                    RefrenceType = typeof(StoreRelease).Name.ToString()
                };
                await _unitOfWork.Repository<StockTransaction, int>().AddAsync(stockTransaction);

                var custodyItem = custoday?.CustodyItems?.FirstOrDefault(ci => ci.ItemId == orderItems.ItemsId);
                if (custodyItem == null)
                {
                    custodyItem = new CustodyItem
                    {
                        CustodyId = custoday.Id,
                        ItemId = orderItems.ItemsId,
                        Quantity = item.Quantity
                    };
                    await _unitOfWork.Repository<CustodyItem, int>().AddAsync(custodyItem);

                }
                else
                {
                    custodyItem.Quantity += item.Quantity;
                    _unitOfWork.Repository<CustodyItem, int>().Update(custodyItem);
                }
            }

            await _unitOfWork.CompleteAsync();

            var Storespec = new StoreItemSpec(storeRelease.Id);
            var storeItem = await _unitOfWork.Repository<StoreReleaseItem, int>().GetWithSpecAsync(Storespec);

            //var OrderitemSpec = new ItemSpec(storeItem.Items.Id);
            //var OrderitemDb = await _unitOfWork.Repository<Items,int>().GetWithSpecAsync(itemSpec);

            var dto = new StoreReleaseDto
            {
                Id = storeRelease.Id,
                UnitName = unit?.UnitName,
                ReceiverName = custoday?.Recipients?.Name,
                ReleaseDate = storeRelease.ReleaseDate,
                DocumentNumber = storeRelease.DocumentNumber,
                StoreReleaseType = "صرف من المستودع",
                FileUrl = _configuration["BASEURL"] + storeRelease.DocumentPath,
                Items = storeRelease.StoreReleaseItems.Select(i => new StoreReleaseItemResponseDto
                {
                    Id = i.Id,
                    ItemName = i.OrderItem?.ItemName,
                    OrderId  = i.OrderId,
                    OrderNumber = storeItem.Order?.OrderNumber,
                    OrderType = i.Order?.OrderType, 
                    OrderItemId = i.ItemId,
                    Quantity = i.Quantity,
                    SerialNumbers = i.SerialNumbers?.Select(sn => sn.SerialNumber).ToList()
                }).ToList()
            };

            return Result<StoreReleaseDto>.Success(dto,"Store Release Saved Successfully.");
        

        }
    }
}
