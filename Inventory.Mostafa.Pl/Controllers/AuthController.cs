using Inventory.Mostafa.Pl.Response.General;
using Inventory.Mostafa.Application.Contract.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Pl.Response.Error;
using Inventory.Mostafa.Application.User.Command.LogIn;
using Inventory.Mostafa.Application.User.Command.Reset;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Mostafa.Pl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthDto>>> LogIn(LogInDto loginDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var loginCommand = loginDto.Adapt<LogInCommand>();
           
            var user = await _mediator.Send(loginCommand);
            if (user == null) return BadRequest(new ErrorResponse(400,user.Message));
            return Ok(new ApiResponse<AuthDto>(true, 200, user.Message, user.Data));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword(ResetPassword dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resetCommand = dto.Adapt<ResetPasswordCommand>();

            var result = await _mediator.Send(resetCommand);
            if (result == null) return BadRequest(new ErrorResponse(400, result.Message));
            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }
    }
}
