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
    public class AllOrdersQueryHandler : IRequestHandler<AllOrdersQuery, Result<Pagination<IEnumerable<OrdersDto>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AllOrdersQueryHandler(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Result<Pagination<IEnumerable<OrdersDto>>>> Handle(AllOrdersQuery request, CancellationToken cancellationToken)
        {
            var parameter = request.Adapt<SpecParameter>();
            var spec = new OrderSpec(parameter);
            var count = new OrderCountSpec(parameter);
            var orders = await _unitOfWork.Repository<Orders,int>().GetAllWithSpecAsync(spec);
            var counts = await _unitOfWork.Repository<Orders, int>().GetCountAsync(count);
            if (orders == null) return Result<Pagination<IEnumerable<OrdersDto>>>.Failure("Faild To Retrive All Orders");

            var ordersDto = orders.Select(o => new OrdersDto()
            {
                Id = o.Id,
                OrderType = o.OrderType,
                OrderNumber = o.OrderNumber,
                Attachment = _configuration["BASEURL"] + o.Attachment,
            });

            var pagintion = new Pagination<IEnumerable<OrdersDto>>(parameter.pageSize,parameter.pageIndex,counts,ordersDto);

            return Result<Pagination<IEnumerable<OrdersDto>>>.Success(pagintion,"All Orders Retrived Successfully.");
        }
    }
}
