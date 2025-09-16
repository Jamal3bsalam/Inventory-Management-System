using Inventory.Mostafa.Domain.Shared;
using Inventory.Mostafa.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Repositories
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Tkey Id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, Tkey> spec);
        Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, Tkey> spec);
        Task<int> GetCountAsync(ISpecification<TEntity, Tkey> spec);

    }
}
