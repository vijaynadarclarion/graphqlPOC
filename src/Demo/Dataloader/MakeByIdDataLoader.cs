using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Data;
using GreenDonut;
using Microsoft.EntityFrameworkCore;
using NUPDemo.Entities;

namespace Demo.Dataloader
{
    public class MakeByIdDataLoader : BatchDataLoader<int, Make>
    {
        private readonly IDbContextFactory<AppReadOnlyDbContext> _dbContextFactory;

        public MakeByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<AppReadOnlyDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, Make>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            var ids = keys.ToList();

            await using var dbContext =
                _dbContextFactory.CreateDbContext();

            //var items = await dbContext.Makes
            //  .AsNoTracking()
            //  .Where(s => ids.Contains(s.Id)).ToListAsync();

            // return items.ToDictionary(t => t.Id);
            return null;
        }
    }
}
