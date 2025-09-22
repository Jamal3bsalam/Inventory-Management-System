using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Command.Update
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileServices<Orders, int> _fileServices;
        private readonly IConfiguration _configuration;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IFileServices<Orders, int> fileServices, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _fileServices = fileServices;
            _configuration = configuration;
        }
        public async Task<Result<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<OrderDto>.Failure("Please Enter A Valid Id.");

            var spec = new OrderSpec(request.Id.Value);
            var order = await _unitOfWork.Repository<Orders, int>().GetWithSpecAsync(spec);

            if (order == null) return Result<OrderDto>.Failure("No Order With This Id To Update.");

            if (!string.IsNullOrEmpty(order.Attachment) && request.File != null)
            {
                _fileServices.Delete(order.Attachment);
                var fileName = _fileServices.Upload(request.File);
                order.Attachment = $"\\Files\\OrdersAttachment\\{fileName}";
            }

            if (!string.IsNullOrEmpty(request.OrderType.Value.ToString()))
            {
                order.OrderType = request.OrderType.Value.ToString();
            }

            if (request.OrderNumber != null || request.OrderNumber != 0)
            {
                order.OrderNumber = request.OrderNumber.ToString();
            }
            _unitOfWork.Repository<Orders, int>().Update(order);
            var result = await _unitOfWork.CompleteAsync();

            var orderDto = new OrderDto()
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderType = order.OrderType,
                SupplierName = order.SupplierName,
                OrderDate = order.OrderDate,
                Attachment = _configuration["BASEURL"] + order.Attachment,
                Items = order.OrderItems.Select(i => new OrderItemDto() { ItemId = i.Id, ItemName = i.ItemName, StockNumber = i.StockNumber, Quantity = i.Quantity, SerialNumbers = i.SerialNumbers.Select(s => s.SerialNumber).ToList() }).ToList(),
            };

            return Result<OrderDto>.Success(orderDto, "Order Updated Successfully.");

        }
    }
}
