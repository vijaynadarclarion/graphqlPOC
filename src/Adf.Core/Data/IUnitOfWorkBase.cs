using System.Threading.Tasks;

namespace Adf.Core.Data
{

    public interface IUnitOfWorkBase<TContext> //where TContext : IBaseDbContext
    {
        // TContext Context { get; }

        //int SaveChanges();
        Task<int> SaveChangesAsync(string currentUserName = null);
        Task<int> SaveChangesWithAuditAsync(string currentUserName = null);
        
        Task OnBeforeSaveChanges();
        Task OnAfterSaveChanges();
    }


}
