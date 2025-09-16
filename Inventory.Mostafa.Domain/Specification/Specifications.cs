using Inventory.Mostafa.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Domain.Specification
{
    public class Specifications<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public List<Expression<Func<TEntity, object>>> Include { get ; set ; } = new List<Expression<Func<TEntity, object>>> ();
        public List<string> IncludeStrings { get; set; } = new List<string>();
        public Expression<Func<TEntity, bool>> Criteria { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }

        public Specifications(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;
        }

        public Specifications()
        {

        }

        public void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }

        public void AddOrderByDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderByDescending = expression;
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPagination = true;
            Skip = skip;
            Take = take;
        }
    }
}
