using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Enums;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Update
{
    public class UpdateUserCommand:IRequest<Result<UserDto>>
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public Roles? Roles { get; set; }
    }
}
