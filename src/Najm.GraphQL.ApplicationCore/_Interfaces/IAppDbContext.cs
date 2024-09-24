using Adf.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Najm.GraphQL.ApplicationCore.Interfaces
{
    public interface IAppDbContext : IDbContextBase
    {
        Task InsertBulkDataAsync<TEntity>(List<TEntity> entities) where TEntity:class;
    }
}
