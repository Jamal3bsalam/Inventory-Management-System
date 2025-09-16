using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Query.AllUsers
{
    public class GetAllUsersQuery : IRequest<Result<IEnumerable<UsersDto>>>
    {
    }
}
