using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Domain.Entities.Order;
using Inventory.Mostafa.Domain.Entities.Store;
using Inventory.Mostafa.Domain.Repositories;
using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using Inventory.Mostafa.Infrastructure.Data.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly InventoryDbContext _context;

        public GenericRepository(InventoryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Tkey Id)
        {
            if(typeof(TEntity) == typeof(OrderItems))
            {
                return (TEntity)(Object)await _context.OrderItems.Include(o => o.SerialNumbers).FirstOrDefaultAsync(s => s.Id == (int)(object)Id);

            }
            if (typeof(TEntity) == typeof(StockTransaction))
            {
                return (TEntity)(Object) await _context.StockTransactions.FirstOrDefaultAsync(s => s.RelatedId == (int)(object)Id);
            }
            return await _context.Set<TEntity>().FindAsync(Id);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
             _context.Remove(entity); 
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public Task<int> GetCountAsync(ISpecification<TEntity, Tkey> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }

        public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, Tkey> spec)
        {
            return SpecificationEvaluator<TEntity, Tkey>.GetQuery(_context.Set<TEntity>(), spec);
        }

        
    }
}
