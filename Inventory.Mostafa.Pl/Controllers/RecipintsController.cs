using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.Store.Command.Add.NewRecipints;
using Inventory.Mostafa.Application.Units.Command.Add;
using Inventory.Mostafa.Application.Units.Query.RecipintsSearch;
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
    public class RecipintsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipintsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<string>>> AddNewRecipints(int unitId , string recipintsName)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var unitCommand = new AddNewRecipintsCommand() { UnitId = unitId,RecipintsName = recipintsName};
            var result = await _mediator.Send(unitCommand);

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RecipintsDtos>>>> RecipintsSearch(string search)
        {
            var getAllRecipints = new SearchQuery() { Search = search };
            var result = await _mediator.Send(getAllRecipints);
            if (result == null) return BadRequest(new ErrorResponse(400, "Faild To Retrive Recipints By This Name"));

            return Ok(new ApiResponse<IEnumerable<RecipintsDtos>>(true, 200, result.Message, result.Data));
        }
    }
}
