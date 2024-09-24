
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Adf.Core.Data
{
    public interface IRepositoryBase<TContext, TEntity>
      //  : Ardalis.Specification.IRepositoryBase<TEntity>
      : IReadWriteRepositoryBase<TEntity>

        //where TContext : IBaseDbContext
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteRangeByQueryAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        
        Task DeleteRangeByQueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default(CancellationToken));

        // TODO: DELETE
        Task<PagedList<TEntity>> ListPagedAsync(int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);


        Task<PagedList<TEntity>> ListPagedAsync(Ardalis.Specification.ISpecification<TEntity> specification, int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);


        Task<PagedList<TResult>> ListPagedAsync<TResult>(Ardalis.Specification.ISpecification<TEntity, TResult> specification, int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        //IQueryable<TEntity> Table { get; }
        //IQueryable<TEntity> TableNoTracking { get; }

        ////    List<TEntity> SearchWithFilters(
        ////            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////            IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////            params Expression<Func<TEntity, object>>[] includes);

        ////    List<TEntity> SearchAndTrackWithFilters(
        ////            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////            IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////            params Expression<Func<TEntity, object>>[] includes);

        ////    PagedList<TEntity> SearchWithFilters(
        ////        int pageNumber,
        ////        int pageSize,
        ////        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////        IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////        params Expression<Func<TEntity, object>>[] includes);

        ////    PagedList<TResult> SearchAndSelectWithFilters<TResult>(
        ////        int pageNumber,
        ////        int pageSize,
        ////        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////        Expression<Func<TEntity, TResult>> selectors,
        ////        IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////       params Expression<Func<TEntity, object>>[] includes
        ////    );
        ////    List<TResult> SearchAndSelectWithFilters<TResult>(
        ////    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////    Expression<Func<TEntity, TResult>> selectors,
        ////    IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////   params Expression<Func<TEntity, object>>[] includes
        ////   );
        ////    IQueryable<TResult> SearchAndSelectWithFilters1<TResult>(
        ////Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        ////Expression<Func<TEntity, TResult>> selectors,
        ////IEnumerable<Expression<Func<TEntity, bool>>> filters = null,
        ////params Expression<Func<TEntity, object>>[] includes
        ////);


        ////    void Delete(object id, bool autoSave = false);
        ////    Task DeleteAsync(object id, bool autoSave = false);

        ////List<TEntity> FromSqlRaw(string sql);
    }
}
