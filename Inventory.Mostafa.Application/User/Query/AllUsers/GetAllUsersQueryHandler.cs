using Inventory.Mostafa.Application.Contract.Auth;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.User.Query.AllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UsersDto>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<IEnumerable<UsersDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDto = new List<UsersDto>();
            foreach (var user in users)
            {
                var currentRole = await _userManager.GetRolesAsync(user);
                foreach (var role in currentRole)
                {
                    usersDto.Add(new UsersDto() { Id = user.Id, Role = role, UserName = user.UserName,FullName = user.FullName,Email = user.Email });
                }
            }


            if (!users.Any()) return Result<IEnumerable<UsersDto>>.Failure("Faild To Get All Users");

            return Result<IEnumerable<UsersDto>>.Success(usersDto, "All Users Retrived Successfully");
        }
    }
}
