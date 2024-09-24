using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Adf.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Najm.GraphQL.ApplicationCore.Interfaces;

namespace Najm.GraphQL.Infrastructure.Data
{
    public partial class AppReadOnlyDbContext : ReadOnlyDbContextBase<AppReadOnlyDbContext>, IAppReadOnlyDbContext
    {
        public AppReadOnlyDbContext(DbContextOptions<AppReadOnlyDbContext> options) : base(options)
        {
            
        }

    }
}
