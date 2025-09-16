using Inventory.Mostafa.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Abstraction.Token
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager);

    }
}
