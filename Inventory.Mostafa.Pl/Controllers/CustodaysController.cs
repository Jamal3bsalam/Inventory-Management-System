using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Application.CustodayRec.Query.All;
using Inventory.Mostafa.Application.Custodays.Command;
using Inventory.Mostafa.Application.Custodays.Query.AllCustodayForSpecUnit;
using Inventory.Mostafa.Application.Custodays.Query.AllTransfers;
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
    public class CustodaysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustodaysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustodaysUnitsDto>>>> GetAllCustodaysForSpecificUnit(int id)
        {
            var getAllCustodays = new CustodaysUnitsQurey() { UnitId = id};
            var result = await _mediator.Send(getAllCustodays);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));


            return Ok(new ApiResponse<IEnumerable<CustodaysUnitsDto>>(true, 200, result.Message, result.Data));
        }

        [HttpPost("transfer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<CustodayDto>>> TransferCustoday(CreateTransactionDto create)
        {
            var transferCustoday = new TransactionCommand() 
            { 
                CustodayId = create.CustodayId,UnitId = create.UnitId,ItemId = create.ItemId,
                NewRecipints = create.NewRecipints,
                TransactionDate = create.TransactionDate, 
                Quantity = create.Quantity, File = create.File
            };

            var result = await _mediator.Send(transferCustoday);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<CustodayDto>(true, 200, result.Message, result.Data));
        }

        [HttpGet("transfers")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<Pagination<IEnumerable<TransactionDto>>>>> GetAllTransaction([FromQuery]StoreReleaseSpecParameter parameter)
        {
            var getAllTransactions = parameter.Adapt<AllTransfersQuery>();
            var result = await _mediator.Send(getAllTransactions);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));


            return Ok(new ApiResponse<Pagination<IEnumerable<TransactionDto>>>(true, 200, result.Message, result.Data));
        }

    }
}
