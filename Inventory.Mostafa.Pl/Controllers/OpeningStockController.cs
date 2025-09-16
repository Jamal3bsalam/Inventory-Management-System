using Inventory.Mostafa.Application.Contract.Opening;
using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Application.Opening.Command.Add;
using Inventory.Mostafa.Application.Opening.Command.Delete;
using Inventory.Mostafa.Application.Opening.Command.Update;
using Inventory.Mostafa.Application.Opening.Query.AllOpeningStock;
using Inventory.Mostafa.Application.Order.Command.Add;
using Inventory.Mostafa.Application.Order.Command.Delete;
using Inventory.Mostafa.Application.Order.Command.Update;
using Inventory.Mostafa.Application.Order.Query.AllOrders;
using Inventory.Mostafa.Domain.Entities.Order;
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
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Mostafa.Pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpeningStockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OpeningStockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<OpeningDtos>>>>> GetAllOpeningStocks([FromQuery]OpeningSpecParameter specParameter)
        {
            var getAllOpeningStocksQuery = specParameter.Adapt<AllOpeningQuery>();
            var opening = await _mediator.Send(getAllOpeningStocksQuery);
            if (opening == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Opening Stocks"));
            return Ok(new ApiResponse<Pagination<IEnumerable<OpeningDtos>>>(true, 200, opening.Message, opening.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<OpeningDto>>> AddOpeningStock(AddOpeningDto addOpening)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var openingCommand = addOpening.Adapt<AddOpeningCommand>();
            var result = await _mediator.Send(openingCommand);

            if(result == null) return BadRequest(new ErrorResponse(400,result.Message));

            return Ok(new ApiResponse<OpeningDto>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<OpeningDto>>> UpdateOpening(UpdateOpeningDto update,int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var openingCommand = new UpdateOpeningCommand() { Id = id, NewQuantity = update.NewQuantity , NewSerialNumbers = update.NewSerialNumbers };
            var order = await _mediator.Send(openingCommand);
            if (order.Data == null) return BadRequest(new ErrorResponse(400, order.Message));


            return Ok(new ApiResponse<OpeningDto>(true, 200, order.Message, order.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteOpening(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var openingCommand = new DeleteOpeningCommand() { Id = id };
            var opening = await _mediator.Send(openingCommand);
            if (opening.Data == null) return BadRequest(new ErrorResponse(400, opening.Message));


            return Ok(new ApiResponse<string>(true, 200, opening.Message, opening.Data));
        }
    }
}
