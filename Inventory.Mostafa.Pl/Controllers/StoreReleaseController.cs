using Inventory.Mostafa.Application.Contract.Order;
using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.Order.Command.Add;
using Inventory.Mostafa.Application.Order.Query.AllOrders;
using Inventory.Mostafa.Application.Store.Command.Add;
using Inventory.Mostafa.Application.Store.Command.Add.File;
using Inventory.Mostafa.Application.Store.Command.Delete;
using Inventory.Mostafa.Application.Store.Command.Update;
using Inventory.Mostafa.Application.Store.Command.Update.File;
using Inventory.Mostafa.Application.Store.Query.AllStoreRelease;
using Inventory.Mostafa.Application.UnitExp.Command.Update;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
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
    public class StoreReleaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StoreReleaseController(IMediator mediator,IConfiguration configuration)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<StoreReleaseDto>>>>> GetAllStoreReleases([FromQuery] StoreReleaseSpecParameter parameter)
        {
            var getAllStoreReleasesQuery = parameter.Adapt<StoreReleasesQuery>();
            var Releases = await _mediator.Send(getAllStoreReleasesQuery);
            if (Releases == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Store Releases"));
            return Ok(new ApiResponse<Pagination<IEnumerable<StoreReleaseDto>>>(true, 200, "StoreRelease Retrived Successfully.", Releases.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<StoreReleaseDto>>> AddStoreRelease(CreateStoreReleaseDto storeReleaseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var storeReleaseCommand = storeReleaseDto.Adapt<AddStoreReleaseCommand>();
            var result = await _mediator.Send(storeReleaseCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));
            return Ok(new ApiResponse<StoreReleaseDto>(true, 200, result.Message, result.Data));
        }

        [HttpPost("attachment")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<Result<string>>> UploadAttachment(IFormFile file)
        {
            var fileCommand = new AddFileCommand() { File = file };
            var result = await _mediator.Send(fileCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<Result<string>>(true, 200, "File Uploaded Successfully", result));
        }

        [HttpPut("attachment")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<Result<string>>> UpdateAttachment(int storeReleaseId, IFormFile file)
        {
            var fileCommand = new UpdateFileCommand() { Id = storeReleaseId, File = file };
            var result = await _mediator.Send(fileCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<Result<string>>(true, 200, "File Updated Successfully", result));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteStoreRelease(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var storeReleaseCommand = new DeleteStoreReleaseCommand() { Id = id};
            var result = await _mediator.Send(storeReleaseCommand);
            if (result.Data == null) return BadRequest(new ErrorResponse(400, result.Message));
            
            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<StoreReleaseDto>>> UpdateStorReleases(UpdateStoreReleaseDto updateStoreRelease, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var storeCommand = updateStoreRelease.Adapt<UpdateStoreReleaseCommand>();
            storeCommand.storeReleaseId = id;
            var result = await _mediator.Send(storeCommand);

            return Ok(new ApiResponse<StoreReleaseDto>(true, 200, result.Message, result.Data));
        }

    }
}
