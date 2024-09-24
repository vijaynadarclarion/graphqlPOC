using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Adf.EntityFrameworkCore;
using Adf.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Najm.GraphQL.Infrastructure.Data;

namespace Najm.GraphQL.Infrastructure.Data
{
    public class PooledReadOnlyRepository<TEntity>
        : EfReadOnlyRepositoryBase<IAppReadOnlyDbContext, TEntity>, IReadOnlyRepository<TEntity>
          where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        IDbContextFactory<AppReadOnlyDbContext> _dbContextFactory;

        public PooledReadOnlyRepository(IDbContextFactory<AppReadOnlyDbContext> dbContextFactory) : base(dbContextFactory.CreateDbContext())
        {
            _dbContextFactory = dbContextFactory;
           // AppReadOnlyDbContext dbContext = (AppReadOnlyDbContext)_dbContext;
            //_dbSet = dbContext.Set<TEntity>();
        }


        public async Task<List<TEntity>> GetAll()
        {
            var data = await _dbSet.AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<List<TEntity>> GetByQuery(Expression<Func<TEntity, bool>> expression)
        {
            var data = await _dbSet.AsNoTracking().Where(expression).ToListAsync();
            return data;
        }

        public async Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            var data = await _dbSet.AsNoTracking().Where(expression).FirstOrDefaultAsync();
            return data;
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return _dbSet.Find(id);
        }

    }
}

