using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.LogIn
{
    public class LogInCommand : IRequest<Result<AuthDto>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
