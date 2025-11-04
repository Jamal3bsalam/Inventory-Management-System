using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Entities.UnitEx;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.CustodaySpecificaion;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Application.Store.Command.Update
{
    public class UpdateStoreReleaseCommandHandler : IRequestHandler<UpdateStoreReleaseCommand, Result<StoreReleaseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UpdateStoreReleaseCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Result<StoreReleaseDto>> Handle(UpdateStoreReleaseCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Get existing store release
            var spec = new StoreReleaseSpec(request.storeReleaseId.Value);
            var oldStoreRelease = await _unitOfWork.Repository<StoreRelease, int>().GetWithSpecAsync(spec);
            if (oldStoreRelease == null)
                return Result<StoreReleaseDto>.Failure($"StoreRelease with Id {request.storeReleaseId} not found");

            // 2️⃣ Rollback old release effect
            foreach (var releaseItem in oldStoreRelease.StoreReleaseItems)
            {
                // rollback order item
                var orderItem = await _unitOfWork.Repository<OrderItems, int>().GetByIdAsync(releaseItem.OrderItemId.Value);
                if (orderItem != null)
                {
                    orderItem.ConsumedQuantity -= releaseItem.Quantity;

                    // rollback serials
                    foreach (var serial in releaseItem.SerialNumbers)
                    {
                        orderItem.SerialNumbers.Add(new ItemSerialNumber
                        {
                            OrderItemsId = orderItem.Id,
                            SerialNumber = serial.SerialNumber
                        });
                    }

                    _unitOfWork.Repository<OrderItems, int>().Update(orderItem);
                }

                // rollback custody
                var custodaySpec = new CustodaySpec(oldStoreRelease.RecipientsId);
                var custody = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(custodaySpec);
                if (custody != null)
                {
                    var custodyItem = custody.CustodyItems?.FirstOrDefault(ci => ci.ItemId == releaseItem.ItemId);
                    if (custodyItem != null)
                    {
                        custodyItem.Quantity -= releaseItem.Quantity;
                        if (custodyItem.Quantity <= 0)
                            _unitOfWork.Repository<CustodyItem, int>().Delete(custodyItem);
                        else
                            _unitOfWork.Repository<CustodyItem, int>().Update(custodyItem);
                    }
                }
            }

            // 3️⃣ Delete old store release
            _unitOfWork.Repository<StoreRelease, int>().Delete(oldStoreRelease);
            await _unitOfWork.CompleteAsync();

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
                ReleaseDate = request.ReleaseDate,
                StoreReleaseItems = new List<StoreReleaseItem>()
            };

            await _unitOfWork.Repository<StoreRelease, int>().AddAsync(storeRelease);
            var saved = await _unitOfWork.CompleteAsync() > 0;
            if (!saved) return Result<StoreReleaseDto>.Failure("Failed to save store release");

            var cSpec = new CustodaySpec(request.ReceiverId);
            var custoday = await _unitOfWork.Repository<Custoday, int>().GetWithSpecAsync(cSpec);

            if (custoday == null)
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

                foreach (var serial in item.SerialNumbers)
                {
                    var storeSerial = orderItems.SerialNumbers.FirstOrDefault(s => s.SerialNumber == serial);
                    if (storeSerial != null)
                    {
                        _unitOfWork.Repository<ItemSerialNumber, int>().Delete(storeSerial);
                    }
                }

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
                    await _unitOfWork.CompleteAsync();
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

            var unitExpenseSpec = new UnitExpenseSpec(oldStoreRelease.Id,true);
            var unitExpenseDb = await _unitOfWork.Repository<UnitExpense,int>().GetWithSpecAsync(unitExpenseSpec);
            if (unitExpenseDb == null) return Result<StoreReleaseDto>.Failure($"No Store Release With This Id: {oldStoreRelease.Id} In Units Expense");
            foreach(var expenseItems in unitExpenseDb.ExpenseItems)
            {
                _unitOfWork.Repository<UnitExpenseItems,int>().Delete(expenseItems);
                await _unitOfWork.CompleteAsync();
            }
            _unitOfWork.Repository<UnitExpense,int>().Delete(unitExpenseDb);
            await _unitOfWork.CompleteAsync();

            var unitExpense = new UnitExpense()
            {
                UnitId = storeRelease.UnitId,
                RecipientsId = storeRelease.RecipientsId,
                ExpenseType = "صرف من المستودع",
                DocumentNumber = storeRelease.DocumentNumber,
                AttachmentUrl = storeRelease.DocumentPath,
                ExpenseDate = storeRelease.ReleaseDate,
                StoreReleaseId = storeRelease.Id,
                ExpenseItems = storeRelease.StoreReleaseItems.Select(u => new UnitExpenseItems()
                {
                    ItemId = u.ItemId,
                    ItemName = u.Items?.ItemsName,
                    Quantity = u.Quantity,
                }).ToList()

            };

            await _unitOfWork.Repository<UnitExpense, int>().AddAsync(unitExpense);
            await _unitOfWork.CompleteAsync();

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
                    OrderId = i.OrderId,
                    OrderNumber = storeItem.Order?.OrderNumber,
                    OrderType = i.Order?.OrderType,
                    OrderItemId = i.OrderItemId,
                    Quantity = i.Quantity,
                    SerialNumbers = i.SerialNumbers?.Select(sn => sn.SerialNumber).ToList()
                }).ToList()
            };

            return Result<StoreReleaseDto>.Success(dto, "Store Release Saved Successfully.");

        }
    }
}
