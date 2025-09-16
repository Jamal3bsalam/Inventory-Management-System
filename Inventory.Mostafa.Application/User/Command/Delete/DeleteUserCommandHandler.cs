using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) return Result<string>.Failure("Please fill all required fields correctly");

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return Result<string>.Failure("User NotFound");


            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user,currentRoles);
            var result =  await _userManager.DeleteAsync(user);
            if(!result.Succeeded) return Result<string>.Failure("Failed To Delete User");

            return Result<string>.Success("User Deleted Successfully.");
        }
    }
}
