using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Add
{
    public class AddUserCommand : IRequest<Result<UserDto>>
    {
        public string? UserName { get; set; }
        public Roles Roles { get; set; }
        public string? Password { get; set; }
    }
}
