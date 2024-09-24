
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Adf.Core.Data
{
    public interface IReadOnlyRepositoryBase<TContext, TEntity>
         // : Ardalis.Specification.IReadRepositoryBase<TEntity>
         : IReadRepositoryBase<TEntity>
        //where TContext : IBaseDbContext
        where TEntity : class
    {

        Task<PagedList<TEntity>> ListPagedAsync(int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

       
        Task<PagedList<TEntity>> ListPagedAsync(Ardalis.Specification.ISpecification<TEntity> specification, int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        
        Task<PagedList<TResult>> ListPagedAsync<TResult>(Ardalis.Specification.ISpecification<TEntity, TResult> specification, int pageNumber, int pageSize, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0, CancellationToken cancellationToken = default);

        
    }
}
