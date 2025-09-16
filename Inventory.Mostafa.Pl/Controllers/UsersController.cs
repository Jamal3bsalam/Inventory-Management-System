using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Application.User.Command.Add;
using Inventory.Mostafa.Application.User.Command.Delete;
using Inventory.Mostafa.Application.User.Command.Update;
using Inventory.Mostafa.Application.User.Query;
using Inventory.Mostafa.Application.User.Query.AllUsers;
using Inventory.Mostafa.Domain.Shared;
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
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UsersDto>>>> GetAllUsers()
        {
            var getAllUsersQuery = new GetAllUsersQuery();
            var result = await _mediator.Send(getAllUsersQuery);

            return Ok(new ApiResponse<IEnumerable<UsersDto>>(true, 200, result.Message, result.Data));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserDto>>> AddUser([FromForm] AddUserDto addUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userCommand = addUser.Adapt<AddUserCommand>();
            var result = await _mediator.Send(userCommand);

            return Ok(new ApiResponse<UserDto>(true, 200, result.Message, result.Data));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser([FromForm] UpdateUserDto updateUser, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var userCommand = updateUser.Adapt<UpdateUserCommand>();
            userCommand.Id = id;
            var result = await _mediator.Send(userCommand);

            return Ok(new ApiResponse<UserDto>(true, 200, result.Message, result.Data));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var deletecommand = new DeleteUserCommand() { Id = id };
            var result = await _mediator.Send(deletecommand);

            return Ok(new ApiResponse<string>(true, 200, result.Message, result.Data));
        }
    }
}
