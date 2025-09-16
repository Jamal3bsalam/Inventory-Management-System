using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Command.Delete
{
    public class DeleteUserCommand:IRequest<Result<string>>
    {
        public int? Id { get; set; }
    }
}
