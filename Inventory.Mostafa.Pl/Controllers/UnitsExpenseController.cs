using Inventory.Mostafa.Application.Contract.Store;
using Inventory.Mostafa.Application.Contract.UnitExp;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.Store.Command.Add;
using Inventory.Mostafa.Application.Store.Command.Delete;
using Inventory.Mostafa.Application.Store.Query.AllStoreRelease;
using Inventory.Mostafa.Application.UnitExp.Command.Add;
using Inventory.Mostafa.Application.UnitExp.Command.Add.File;
using Inventory.Mostafa.Application.UnitExp.Command.Delete;
using Inventory.Mostafa.Application.UnitExp.Command.Update;
using Inventory.Mostafa.Application.UnitExp.Command.Update.File;
using Inventory.Mostafa.Application.UnitExp.Query.AllUnitExpense;
using Inventory.Mostafa.Application.Units.Command.Delete;
using Inventory.Mostafa.Application.Units.Command.Update;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification.Store;
using Inventory.Mostafa.Domain.Specification.UnitExp;
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
    public class UnitsExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UnitsExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<UnitExpensDto>>>>> GetAllUnitExpenses([FromQuery] UnitExpenseParameter parameter)
        {
            var getAllUnitExpensesQuery = parameter.Adapt<UnitsExpensesQuery>();
            getAllUnitExpensesQuery.Search = parameter.Search;
            var Releases = await _mediator.Send(getAllUnitExpensesQuery);
            if (Releases == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Orders"));
            return Ok(new ApiResponse<Pagination<IEnumerable<UnitExpensDto>>>(true, 200, "StoreRelease Retrived Successfully.", Releases.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<UnitExpensDto>>> AddUnitExpense(CreateUnitExpenseDto unitExpenseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var unitExpenseCommand = unitExpenseDto.Adapt<AddUnitExpenseCommand>();
            var result = await _mediator.Send(unitExpenseCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));
            return Ok(new ApiResponse<UnitExpensDto>(true, 200, result.Message, result.Data));
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
        public async Task<ActionResult<Result<string>>> UpdateAttachment(int unitExpenseId,IFormFile file)
        {
            var fileCommand = new UpdateFileCommand() { Id = unitExpenseId,File = file };
            var result = await _mediator.Send(fileCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<Result<string>>(true, 200, "File Updated Successfully", result));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUnitExpenses(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var unitExpenseCommand = new DeleteUnitExpenseCommand() { Id = id };
            var result = await _mediator.Send(unitExpenseCommand);
            if (result.Data == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UnitExpensDto>>> UpdateUnit(CreateUnitExpenseDto updateUnit, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var unitCommand = updateUnit.Adapt<UpdateUnitExpenseCommand>();
            unitCommand.UnitExpenseId = id;
            var result = await _mediator.Send(unitCommand);

            return Ok(new ApiResponse<UnitExpensDto>(true, 200, result.Message, result.Data));
        }
    }
}
