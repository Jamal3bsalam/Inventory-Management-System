using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.CustodayRec.Command.Add;
using Inventory.Mostafa.Application.CustodayRec.Command.Delete;
using Inventory.Mostafa.Application.CustodayRec.Command.Update;
using Inventory.Mostafa.Application.CustodayRec.Query;
using Inventory.Mostafa.Application.ITem.Command.Add;
using Inventory.Mostafa.Application.ITem.Command.Delete;
using Inventory.Mostafa.Application.ITem.Command.Update;
using Inventory.Mostafa.Application.ITem.Query.AllItems;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
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
    public class CustodayRecordController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustodayRecordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RecordDto>>>> GetAllRecords()
        {
            var getAllRecord = new AllRecordQuery();
            var result = await _mediator.Send(getAllRecord);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));


            return Ok(new ApiResponse<IEnumerable<RecordDto>>(true, 200, result.Message, result.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<RecordDto>>> AddRecord([FromForm] AddRecordDto addRecord)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var recordCommand = new AddRecordCommand() { Notic = addRecord.Notic,Date = addRecord.Date , File = addRecord.File};
            var result = await _mediator.Send(recordCommand);

            return Ok(new ApiResponse<RecordDto>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<RecordDto>>> UpdateRecord([FromForm] UpdateRecordDto updateRecord,int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var recordCommand = new UpdateRecordCommand() {RecordId = id ,Notic = updateRecord.Notic,Date =  updateRecord.Date , File = updateRecord.File};
            var result = await _mediator.Send(recordCommand);

            return Ok(new ApiResponse<RecordDto>(true, 200, result.Message, result.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRecord(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var deleteRecord = new DeleteRecordCommand() { Id = id };
            var result = await _mediator.Send(deleteRecord);

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }
    }
}
