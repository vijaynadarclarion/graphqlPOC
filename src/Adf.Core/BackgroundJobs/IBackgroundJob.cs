// <copyright file="IBackgroundTasks.cs" company="Usama Nada">
//   No Copyright .. Copy, Share, and Evolve.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Adf.Core.BackgroundJobs
{
    /// <summary>
    /// The BackgroundTasks interface.
    /// </summary>
    public interface IBackgroundJob
    {
        string CronExpression { get; }
        Task Execute();
        Task ExecuteFireAndForgetJob();
        Task ExecuteDelayedJob();
        Task ExecuteRecurringJob();
        Task ExecuteContinuationsJob();
        Task ExecuteParallelJob();
        

    }
}
