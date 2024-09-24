using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Adf.Core.Data;

namespace Najm.GraphQL.ApplicationCore.Interfaces
{
    public interface IReadOnlyRepository<TEntity> : IReadOnlyRepositoryBase<IAppReadOnlyDbContext, TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetAll();

        Task<List<TEntity>> GetByQuery(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "");

        TEntity GetByID(object id);
    }
}
