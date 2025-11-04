using Inventory.Mostafa.Domain.Repositories;
using Inventory.Mostafa.Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Application.Abstraction.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IGenericRepository<TEntity, Tkey> Repository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
