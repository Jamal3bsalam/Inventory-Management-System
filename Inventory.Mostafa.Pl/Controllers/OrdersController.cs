using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.ITem.Query.AllItems;
using Inventory.Mostafa.Application.Order.Command.Add;
using Inventory.Mostafa.Application.Order.Command.Add.Attachment;
using Inventory.Mostafa.Application.Order.Command.Delete;
using Inventory.Mostafa.Application.Order.Command.Update;
using Inventory.Mostafa.Application.Order.Query.AllOrders;
using Inventory.Mostafa.Application.Order.Query.AllOrdersForSpecificType;
using Inventory.Mostafa.Application.Units.Command.Add;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using Inventory.Mostafa.Domain.Specification.OrderSpecification;
using Inventory.Mostafa.Pl.Attributes;
using Inventory.Mostafa.Pl.Response.Error;
using Inventory.Mostafa.Pl.Response.General;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory.Mostafa.Pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator,IFileServices<Orders,int> fileServices)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<OrderDto>>>>> GetAllOrders([FromQuery] SpecParameter specParameter)
        {
            var getAllOrdersQuery = specParameter.Adapt<AllOrdersQuery>();
            var orders = await _mediator.Send(getAllOrdersQuery);
            if (orders == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Orders"));
            return Ok(new ApiResponse<Pagination<IEnumerable<OrderDto>>>(true, 200, "Orders Retrived Successfully.",orders.Data));
        }

        [HttpGet("Type")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAllOrdersForSpecificType([FromQuery] OrderType orderType)
        {
            var getAllOrdersQuery = new OrdersQuery() { OrderType = orderType};
            var orders = await _mediator.Send(getAllOrdersQuery);
            if (orders == null) return BadRequest(new ErrorResponse(400, orders.Message));
            return Ok(new ApiResponse<IEnumerable<OrderDto>>(true, 200, orders.Message, orders.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> AddOrder(CreateOrderDto addOrder)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var orderCommand = addOrder.Adapt<AddOrderCommand>();
            var result = await _mediator.Send(orderCommand);

            return Ok(new ApiResponse<OrderDto>(true, 200, result.Message, result.Data));
        }

        [HttpPost("attachment")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<Result<string>>> UploadAttachment(IFormFile file)
        {
            var fileCommand = new AddFileCommand() { File = file };
            var result = await _mediator.Send(fileCommand);


            return Ok(new ApiResponse<Result<string>>(true,200,"File Uploaded Successfully",result));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrder(UpdateOrderDto update)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var orderCommand = new UpdateOrderCommand() { Id = update.Id,OrderNumber = update.OrderNumber,OrderType =update.OrderType, File = update.File};
            var order = await _mediator.Send(orderCommand);
            if (order.Data == null) return BadRequest(new ErrorResponse(400,order.Message));


            return Ok(new ApiResponse<OrderDto>(true, 200,order.Message, order.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteOrder(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var orderCommand = new DeleteOrderCommand() { Id = id};
            var order = await _mediator.Send(orderCommand);
            if (order.Data == null) return BadRequest(new ErrorResponse(400, order.Message));


            return Ok(new ApiResponse<string>(true, 200, order.Message, order.Data));
        }


    }
}
