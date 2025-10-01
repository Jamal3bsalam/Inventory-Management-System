using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Add
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AddOrderCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderDto>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {

            var order = new Orders()
            {
                OrderNumber = request.OrderNumber,
                OrderType = request.OrderType.Value.ToString(),
                SupplierName = request.SupplierName,
                OrderDate = request.OrderDate,
                Attachment = $"\\Files\\OrdersAttachment\\{request.Attachment}",
                OrderItems = new List<OrderItems>()
            };
            foreach (var items in request.Items)
            {
                var spec = new ItemSpec(items.ItemId.Value);
                var item = await _unitOfWork.Repository<Items, int>().GetWithSpecAsync(spec);
                var orderItem = new OrderItems
                {
                    ItemsId = items.ItemId,
                    ItemName = item.ItemsName,
                    StockNumber = item.StockNumber,
                    Quantity = items.Quantity,
                    ConsumedQuantity = 0,
                    SerialNumbers = items.SerialNumbers
                        .Select(s => new ItemSerialNumber { SerialNumber = s })
                        .ToList()
                };

                order.OrderItems.Add(orderItem);
            }

            await _unitOfWork.Repository<Orders, int>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();

            var orderDto = new OrderDto()
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderType = order.OrderType,
                SupplierName = order.SupplierName,
                OrderDate = order.OrderDate,
                Attachment = _configuration["BASEURL"] + order.Attachment,
                Items = order.OrderItems.Select(i => new OrderItemDto() { ItemId = i.Id, ItemName = i.ItemName, StockNumber = i.StockNumber, Quantity = i.Quantity,ConsumedQuantity = i.ConsumedQuantity,RemainingQuantity = (i.Quantity - i.ConsumedQuantity) ,SerialNumbers = i.SerialNumbers.Select(s => s.SerialNumber).ToList() }).ToList(),
            };

            if (result <= 0) return Result<OrderDto>.Failure("Faild To Add Order.");

            return Result<OrderDto>.Success(orderDto, "Order Added Successfully.");
        }


    }
}
