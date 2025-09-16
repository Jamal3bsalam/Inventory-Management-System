using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Context
{
    public class InventoryDbContext:IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public DbSet<Items> Items { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Recipients> Recipients { get; set; }
    }
}
