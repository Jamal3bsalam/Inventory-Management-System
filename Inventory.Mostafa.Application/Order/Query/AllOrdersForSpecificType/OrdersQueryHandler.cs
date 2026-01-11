using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NHibernate.Criterion;

namespace Inventory.Mostafa.Application.Order.Query.AllOrdersForSpecificType
{
    public class OrdersQueryHandler : IRequestHandler<OrdersQuery, Result<IEnumerable<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public OrdersQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<IEnumerable<OrderDto>>> Handle(OrdersQuery request, CancellationToken cancellationToken)
        {
            var spec = new OrderSpec(request.OrderType.Value);
            var orders = await _unitOfWork.Repository<Orders, int>().GetAllWithSpecAsync(spec);
            if (orders == null) return Result<IEnumerable<OrderDto>>.Failure("Faild To Retrive All Orders With This Type");

            //var ordersDto = orders.Select(o => new OrdersDto()
            //{
            //    Id = o.Id,
            //    OrderType = o.OrderType,
            //    OrderNumber = o.OrderNumber,
            //    Attachment = _configuration["BASEURL"] + o.Attachment,
            //});
            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderType = o.OrderType,
                SupplierName = o.SupplierName,
                RecipintName = o.RecipintName,  
                OrderDate = o.OrderDate,
                Attachment = _configuration["BASEURL"] + o.Attachment,
                Items = o.OrderItems.Select(i => new OrderItemDto() { ItemId = i.Id, ItemName = i.ItemName, StockNumber = i.StockNumber, Quantity = i.Quantity,ConsumedQuantity = i.ConsumedQuantity,RemainingQuantity = (i.Quantity - i.ConsumedQuantity) ,SerialNumbers = i.SerialNumbers.Select(s => s.SerialNumber).ToList() }).ToList(),
            });


            return Result<IEnumerable<OrderDto>>.Success(ordersDto, "All Orders Retrived Successfully.");
        }
    }
}
