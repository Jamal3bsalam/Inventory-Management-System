using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Inventory.Mostafa.Application.Order.Query.OrdersForSpecificRecipints
{
    public class GetAllOrdersByRecipintQueryHandler : IRequestHandler<GetAllOrdersByRecipintQuery, Result<OrdersForRecipientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GetAllOrdersByRecipintQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<OrdersForRecipientResponseDto>> Handle(GetAllOrdersByRecipintQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RecipintName))
            {
                return Result<OrdersForRecipientResponseDto>.Failure("Recipint name cannot be null or empty.");
            }

            var orderSpec = new OrderSpec(request.RecipintName);
            var orders = await _unitOfWork.Repository<Orders,int>().GetAllWithSpecAsync(orderSpec);
            if (orders == null || !orders.Any())
            {
                return Result<OrdersForRecipientResponseDto>.Failure("No orders found for the specified recipient.");
            }

            var ordersDto = orders.Select(order => new OrdersForRecipintDto
            {
                OrderId = order.Id,
                SupplierName = order.SupplierName,
                RecipintName = order.RecipintName,
                OrderNumber = order.OrderNumber,
                OrderType = order.OrderType,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
                DocumentUrl = string.IsNullOrEmpty(order.Attachment) ? null : _configuration["BASEURL"] + order.Attachment,
                Items = order.OrderItems.Select(i => new OrderItemForRecipintDto() { ItemName = i.ItemName ,TotalQuantity = i.Quantity,ConsumedQuantity = i.ConsumedQuantity}).ToList(),
            }).ToList();

            var orderTypesCount = new OrderTypesCountDto
            {
                EndorsementCont = orders.Count(o => o.OrderType == OrderType.تعميد.ToString()),
                SupportCount = orders.Count(o => o.OrderType == OrderType.دعم.ToString()),
                PurchaseCount = orders.Count(o => o.OrderType == OrderType.شراء.ToString()),
                ReturnCount = orders.Count(o => o.OrderType == OrderType.رجيع.ToString()),
                TotalCount = orders.Count()
            };

            var response = new OrdersForRecipientResponseDto()
            {
                OrderTypesCount = orderTypesCount,
                Orders = ordersDto
            };

            return Result<OrdersForRecipientResponseDto>.Success(response, "All Orders Retrived Successfully.");
        }
    }
}
