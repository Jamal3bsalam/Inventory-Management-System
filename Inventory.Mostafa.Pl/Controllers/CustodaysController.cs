using Inventory.Mostafa.Application.Contract.CustodayDtos;
using Inventory.Mostafa.Application.Contract.CustodayRec;
using Inventory.Mostafa.Application.Contract.Units;
using Inventory.Mostafa.Application.CustodayRec.Query.All;
using Inventory.Mostafa.Application.Custodays.Command;
using Inventory.Mostafa.Application.Custodays.Command.File;
using Inventory.Mostafa.Application.Custodays.Command.File.Add;
using Inventory.Mostafa.Application.Custodays.Command.File.Update;
using Inventory.Mostafa.Application.Custodays.Query.AllCustodayForSpecUnit;
using Inventory.Mostafa.Application.Custodays.Query.AllTransfers;
using Inventory.Mostafa.Application.Custodays.Query.RecipintsCustoday;
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
        public async Task<ActionResult<ApiResponse<List<CustodayDto>>>> TransferCustoday(CreateTransactionDto create)
        {
            var transferCustoday = new TransactionCommand()
            {
                UnitId = create.UnitId,
                NewRecipints = create.NewRecipints, FileName = create.FileName,TransactionDate = create.TransactionDate,
                Items = create.Items
            };
            var result = await _mediator.Send(transferCustoday);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<List<CustodayDto>>(true, 200, result.Message, result.Data));
        }

        [HttpPost("transfer/attachment")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<Result<string>>> UploadAttachment(IFormFile file)
        {
            var fileCommand = new AddFileCommand() { File = file };
            var result = await _mediator.Send(fileCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<Result<string>>(true, 200, "File Uploaded Successfully", result));
        }
        [HttpPut("transfer/attachment")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<Result<string>>> UpdateAttachment(int id,IFormFile file)
        {
            var fileCommand = new UpdateFileCommand() { Id = id,File = file };
            var result = await _mediator.Send(fileCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));

            return Ok(new ApiResponse<Result<string>>(true, 200, "File Updated Successfully", result));
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

        [HttpGet("recipints/report")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        [Cashed(60)]
        public async Task<ActionResult<ApiResponse<RecipintsCustodayDto>>> GetAllCustodayForSpecificRecipintsReport(int unitId,int recipintsId)
        {
            var getAllTransactions = new AllRecipintsCustodayQuery()
            {
                UnitId = unitId, RecipintsId = recipintsId
            };
            var result = await _mediator.Send(getAllTransactions);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));


            return Ok(new ApiResponse<IEnumerable<RecipintsCustodayDto>>(true, 200, result.Message, result.Data));
        }


    }
}
