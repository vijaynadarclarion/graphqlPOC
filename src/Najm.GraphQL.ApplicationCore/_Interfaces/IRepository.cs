using Adf.Core.Data;

namespace Najm.GraphQL.ApplicationCore.Interfaces
{
    public interface IRepository<TEntity> : IRepositoryBase<IAppDbContext, TEntity> where TEntity : class
    {
    }
}
