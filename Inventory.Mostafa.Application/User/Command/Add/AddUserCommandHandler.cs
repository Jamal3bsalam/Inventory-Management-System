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

namespace Inventory.Mostafa.Application.User.Command.Add
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<UserDto>>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<UserDto>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Roles.ToString())) return Result<UserDto>.Failure("Please fill all required fields correctly");
            if (await CheckUserNameExist(request.UserName)) return Result<UserDto>.Failure("UserName Already Exist");

            var userEmail = await _userManager.FindByEmailAsync(request.Email);
            if(userEmail != null)
            {
                return Result<UserDto>.Failure("Email Already Exist.");
            }

            var user = new AppUser()
            {
                UserName = request.UserName, 
                Email = request.Email,
                FullName = request.FullName
            };

            await _userManager.CreateAsync(user,request.Password);
            await _userManager.AddToRoleAsync(user,request.Roles.ToString());

            var userDto = user.Adapt<UserDto>();
            userDto.Role = request.Roles.ToString();
            return Result<UserDto>.Success(userDto,"User Added Successfully");    
        }
        public async Task<bool> CheckUserNameExist(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null) return true;
            return false;
        }

        
    }
}
