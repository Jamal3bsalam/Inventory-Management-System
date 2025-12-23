using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.ITem.Command.Add;
using Inventory.Mostafa.Application.ITem.Command.Delete;
using Inventory.Mostafa.Application.ITem.Command.Update;
using Inventory.Mostafa.Application.ITem.Query.AllItems;
using Inventory.Mostafa.Application.User.Command.Add;
using Inventory.Mostafa.Application.User.Command.Delete;
using Inventory.Mostafa.Application.User.Command.Update;
using Inventory.Mostafa.Application.User.Query.AllUsers;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using Inventory.Mostafa.Domain.Specification.ITemSpecification;
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
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<ItemDto>>>>> GetAllItems([FromQuery]ItemSpecParameter specParameter)
        {
            var getAllItemsQuery = specParameter.Adapt<AllItemsQuery>();
            var result = await _mediator.Send(getAllItemsQuery);
            if (result == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Items"));


            return Ok(new ApiResponse<Pagination<IEnumerable<ItemDto>>>(true, 200, "Items Retrived Successfully.", result));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> AddItem([FromForm] AddItemDto addItem)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var itemCommand = addItem.Adapt<AddItemCommand>();
            var result = await _mediator.Send(itemCommand);

            return Ok(new ApiResponse<ItemDto>(true, 200, result.Message, result.Data));
        }

        [HttpPut("id")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> UpdateItem([FromForm] ItemDto updateItem)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var itemCommand = updateItem.Adapt<UpdateItemCommand>();
            var result = await _mediator.Send(itemCommand);

            return Ok(new ApiResponse<ItemDto>(true, 200, result.Message, result.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteItem(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var deleteItem = new DeleteItemCommand() { Id = id };
            var result = await _mediator.Send(deleteItem);

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }
    }
}
