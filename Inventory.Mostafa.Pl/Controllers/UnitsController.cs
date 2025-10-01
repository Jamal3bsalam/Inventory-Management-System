using Inventory.Mostafa.Application.Contract.ITem;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.ITem.Command.Add;
using Inventory.Mostafa.Application.ITem.Command.Delete;
using Inventory.Mostafa.Application.ITem.Command.Update;
using Inventory.Mostafa.Application.ITem.Query.AllItems;
using Inventory.Mostafa.Application.Units.Command.Add;
using Inventory.Mostafa.Application.Units.Command.Delete;
using Inventory.Mostafa.Application.Units.Command.Update;
using Inventory.Mostafa.Application.Units.Query.AllUnits;
using Inventory.Mostafa.Application.Units.Query.RecipintsSearch;
using Inventory.Mostafa.Domain.Entities.Identity;
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
using Unit = Inventory.Mostafa.Domain.Entities.Identity.Unit;


namespace Inventory.Mostafa.Pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UnitsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<UnitDto>>>>> GetAllUnits([FromQuery] SpecParameter specParameter)
        {
            var getAllUnitsQuery = specParameter.Adapt<AllUnitsQuery>();
            var result = await _mediator.Send(getAllUnitsQuery);
            if (result == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive All Units"));

            return Ok(new ApiResponse<Pagination<IEnumerable<UnitDto>>>(true, 200, "Units Retrived Successfully.", result));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<UnitDto>>> AddUnit(AddUnitDto addUnit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var unitCommand = addUnit.Adapt<AddUnitCommand>();
            var result = await _mediator.Send(unitCommand);

            return Ok(new ApiResponse<UnitDto>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UnitDto>>> UpdateUnit(AddUnitDto updateUnit,int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var unitCommand = updateUnit.Adapt<UpdateUnitCommand>();
            unitCommand.Id = id;
            var result = await _mediator.Send(unitCommand);

            return Ok(new ApiResponse<UnitDto>(true, 200, result.Message, result.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUnit(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var deleteUnit = new DeleteUnitCommand() { Id = id };
            var result = await _mediator.Send(deleteUnit);

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }
    }
}
