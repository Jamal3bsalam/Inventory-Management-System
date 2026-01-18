using Inventory.Mostafa.Domain.Entities;
using Inventory.Mostafa.Domain.Entities.AssetsReturns;
using Inventory.Mostafa.Domain.Entities.CustodayTables;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Opening;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
        //Order
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<ItemSerialNumber> ItemSerialNumbers { get; set; }
        //Opening Stock
        public DbSet<OpeningSerialNumber> OpeningSerialNumbers { get; set; }
        public DbSet<OpeningStock> OpeningStocks { get; set; }
        //Store
        public DbSet<StoreRelease> StoreReleases { get; set; }
        public DbSet<StoreReleaseItem> StoreReleaseItems { get; set; }
        public DbSet<ReleaseItemSerialNumber> ReleaseItemSerialNumbers { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        //Custoday
        public DbSet<Custoday> Custodays { get; set; }
        public DbSet<CustodyItem> CustodyItems { get; set; }

        public DbSet<WriteOff> WriteOff { get; set; }
        public DbSet<Returns> Returns { get; set; }
        public DbSet<CustodayTransfers> CustodayTransfers { get; set; }
        public DbSet<CustodyItemUnitExpense> CustodyItemUnitExpense { get; set; }
    }
}
