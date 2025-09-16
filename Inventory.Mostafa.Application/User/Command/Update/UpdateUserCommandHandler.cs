using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Roles.ToString())) return Result<UserDto>.Failure("Please fill all required fields correctly");
            if (await CheckUserNameExist(request.UserName)) return Result<UserDto>.Failure("UserName Already Exist");

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return Result<UserDto>.Failure("User NotFound");

            if(!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;
            if(request.Roles != null)
            {
                var currentRole = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user,currentRole);
            }
            await _userManager.UpdateAsync(user);
            await _userManager.AddToRoleAsync(user,request.Roles.ToString());

            var userDto = user.Adapt<UserDto>();
            userDto.Role = request.Roles.ToString();

            return Result<UserDto>.Success(userDto,"User Updated Successfully.");

        }

        public async Task<bool> CheckUserNameExist(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null) return true;
            return false;
        }
    }
}
