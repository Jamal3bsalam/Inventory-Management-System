using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Application.Order.Command.Delete;
using Inventory.Mostafa.Application.Return.Command.Add;
using Inventory.Mostafa.Application.Return.Command.Delete;
using Inventory.Mostafa.Application.Return.Command.Update;
using Inventory.Mostafa.Application.Return.Query.AllReturns;
using Inventory.Mostafa.Application.Store.Query.AllStoreRelease;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
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
    public class ReturnsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReturnsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<ReturnDto>>>>> GetAllReturns([FromQuery] StoreReleaseSpecParameter parameter)
        {
            var getAllReturnsQuery = parameter.Adapt<ReturnsQuery>();
            var Releases = await _mediator.Send(getAllReturnsQuery);
            if (Releases == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Returns"));
            return Ok(new ApiResponse<Pagination<IEnumerable<ReturnDto>>>(true, 200, "Returns Retrived Successfully.", Releases.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ReturnDto>>> AddReturn(CreateReturn createReturn)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var returnCommand = new AddReturnCommand() { UnitId = createReturn.UnitId,RecipintsId = createReturn.RecipintsId , StoreReleaseItemId = createReturn.StoreReleaseItemId, Document = createReturn.Document , Quantity = createReturn.Quantity , Reason = createReturn.Reason};
            var returns = await _mediator.Send(returnCommand);
            if(returns.Data == null) return BadRequest(new ErrorResponse(400,returns.Message));

            return Ok(new ApiResponse<ReturnDto>(true, 200, returns.Message, returns.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<ReturnDto>>> UpdateReturn(int id, UpdateReturnDto updateReturn)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var returnCommand = new UpdateReturnCommand() {ReturnId = id,File = updateReturn.Document, Quantity = updateReturn.Quantity, Reason = updateReturn.Reason };
            var returns = await _mediator.Send(returnCommand);
            if (returns.Data == null) return BadRequest(new ErrorResponse(400, returns.Message));

            return Ok(new ApiResponse<ReturnDto>(true, 200, returns.Message, returns.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteReturn(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var returnCommand = new DeleteReturnCommand() { Id = id };
            var returns = await _mediator.Send(returnCommand);
            if (returns.Data == null) return BadRequest(new ErrorResponse(400, returns.Message));


            return Ok(new ApiResponse<string>(true, 200, returns.Message, returns.Data));
        }
    }
}
