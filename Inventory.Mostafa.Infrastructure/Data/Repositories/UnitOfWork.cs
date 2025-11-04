using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Domain.Repositories;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _context;
        private readonly Hashtable _repositories;

        public UnitOfWork(InventoryDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<TEntity, Tkey>(_context);
                _repositories.Add(type, repo);
            }

            return _repositories[type] as IGenericRepository<TEntity, Tkey>;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();

        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
