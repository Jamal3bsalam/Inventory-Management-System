using Inventory.Mostafa.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Seed
{
    public class InventoryDbSeed
    {
        public async static Task SeedAppUser(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    UserName = "Admin",
                };
                await _userManager.CreateAsync(user, "Admin@123");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

        }
        public async static Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole<int>("User"));

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
        }
    }
}
