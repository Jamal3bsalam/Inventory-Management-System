using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Reset
{
    public class ResetPasswordCommand:IRequest<Result<string>>
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
