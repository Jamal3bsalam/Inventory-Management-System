using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Reset
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == 0 || request.UserId == null || string.IsNullOrEmpty(request.Password)) return Result<string>.Failure("Please Enter Valid UserId");

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if(user == null)
            {
                return Result<string>.Failure("User Not Found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                return Result<string>.Failure("Faild To Reset Password.");
            }

            return Result<string>.Success("Password Changed Succssfully.");
        }
    }
}
