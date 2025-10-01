using Inventory.Mostafa.Application.Contract.Return;
using Inventory.Mostafa.Application.Contract.writeOff;
using Inventory.Mostafa.Application.Return.Command.Add;
using Inventory.Mostafa.Application.Return.Command.Delete;
using Inventory.Mostafa.Application.Return.Command.Update;
using Inventory.Mostafa.Application.Return.Query.AllReturns;
using Inventory.Mostafa.Application.Write.Command.Add;
using Inventory.Mostafa.Application.Write.Command.Delete;
using Inventory.Mostafa.Application.Write.Command.Update;
using Inventory.Mostafa.Application.Write.Query.AllWriteOff;
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
    public class WriteOffsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WriteOffsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<WriteOffDto>>>>> GetAllReturns([FromQuery] StoreReleaseSpecParameter parameter)
        {
            var getAllWriteOffsQuery = parameter.Adapt<WriteOffsQuery>();
            var writeOffs = await _mediator.Send(getAllWriteOffsQuery);
            if (writeOffs == null) return BadRequest(new ErrorResponse(400, writeOffs.Message));
            return Ok(new ApiResponse<Pagination<IEnumerable<WriteOffDto>>>(true, 200, writeOffs.Message, writeOffs.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<WriteOffDto>>> AddWriteOff(CreateWriteOff createWriteOff)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var writeOffCommand = new WriteOffCommand() { UnitId = createWriteOff.UnitId, RecipintsId = createWriteOff.RecipintsId,ReturnsId = createWriteOff.ReturnId,Documet = createWriteOff.Document, Quantity = createWriteOff.Quantity};
            var writeOff = await _mediator.Send(writeOffCommand);
            if (writeOff.Data == null) return BadRequest(new ErrorResponse(400, writeOff.Message));

            return Ok(new ApiResponse<WriteOffDto>(true, 200, writeOff.Message, writeOff.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<WriteOffDto>>> UpdateWriteOff(int id,  UpdateWriteOff updateWriteOff)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var writeCommand = new UpdateWriteOffCommand() { Id = id, File = updateWriteOff.File, Quantity = updateWriteOff.Quantity};
            var returns = await _mediator.Send(writeCommand);
            if (returns.Data == null) return BadRequest(new ErrorResponse(400, returns.Message));

            return Ok(new ApiResponse<WriteOffDto>(true, 200, returns.Message, returns.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteWriteOff(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var deleteCommand = new DeleteWriteOffCommand() { Id = id };
            var returns = await _mediator.Send(deleteCommand);
            if (returns.Data == null) return BadRequest(new ErrorResponse(400, returns.Message));


            return Ok(new ApiResponse<string>(true, 200, returns.Message, returns.Data));
        }


    }
}
