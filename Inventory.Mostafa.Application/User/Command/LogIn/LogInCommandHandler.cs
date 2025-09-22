using Inventory.Mostafa.Application.Abstraction.Token;
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

namespace Inventory.Mostafa.Application.User.Command.LogIn
{
    public class LogInCommandHandler : IRequestHandler<LogInCommand, Result<AuthDto>>
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LogInCommandHandler(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<Result<AuthDto>> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password)) return Result<AuthDto>.Failure("Invalid username or password");
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return Result<AuthDto>.Failure("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Result<AuthDto>.Failure("Invalid username or password");

            var authDto = new AuthDto()
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };

            return Result<AuthDto>.Success(authDto, "LogIn Completed Successfully");
        }
    }
}
