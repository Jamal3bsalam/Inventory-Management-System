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
            if (request.Id == null) return Result<UserDto>.Failure("Please fill all required fields correctly");

            if (!string.IsNullOrEmpty(request.UserName))
                if (await CheckUserNameExist(request.UserName)) return Result<UserDto>.Failure("UserName Already Exist");

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return Result<UserDto>.Failure("User NotFound");

            if(!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;

            if (!string.IsNullOrEmpty(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email; 
            }

            if (request.Roles != null)
            {
                var currentRole = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user,currentRole);
                await _userManager.AddToRoleAsync(user,request.Roles.ToString());
            }
            await _userManager.UpdateAsync(user);

            var userDto = user.Adapt<UserDto>();
            var role = await _userManager.GetRolesAsync(user);
            userDto.Role = role.FirstOrDefault();

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
