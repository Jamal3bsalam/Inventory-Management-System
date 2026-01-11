using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Domain.Specification.UnitSpecification;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Order.Query.AllOrders
{
    public class AllOrdersQueryHandler : IRequestHandler<AllOrdersQuery, Result<Pagination<IEnumerable<OrderDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllOrdersQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<OrderDto>>>> Handle(AllOrdersQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<SpecParameter>();
            var spec = new OrderSpec(parameter);
            var count = new OrderCountSpec(parameter);
            var orders = await _unitOfWork.Repository<Orders,int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Orders, int>().GetCountAsync(count);
            if (orders == null) return Result<Pagination<IEnumerable<OrderDto>>>.Failure("Faild To Retrive All Orders");

            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderType = o.OrderType,
                SupplierName = o.SupplierName,
                RecipintName = o.RecipintName,
                OrderDate = o.OrderDate,
                Attachment = _configuration["BASEURL"] + o.Attachment,
                Items = o.OrderItems.Select(i => new OrderItemDto() { ItemId = i.Id, ItemName = i.ItemName, StockNumber = i.StockNumber, Quantity = i.Quantity,ConsumedQuantity = i.ConsumedQuantity, RemainingQuantity = (i.Quantity - i.ConsumedQuantity), SerialNumbers = i.SerialNumbers.Select(s => s.SerialNumber).ToList() }).ToList(),
            });

            var pagintion = new Pagination<IEnumerable<OrderDto>>(parameter.pageSize,parameter.pageIndex,counts,ordersDto);

            return Result<Pagination<IEnumerable<OrderDto>>>.Success(pagintion,"All Orders Retrived Successfully.");
        }
    }
}
