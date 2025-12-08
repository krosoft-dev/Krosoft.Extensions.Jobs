using Krosoft.Extensions.Jobs.Hangfire.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Interfaces;

public interface IJobManager
{
    Task AddJobAsync(JobContext jobContext, CancellationToken cancellationToken);
    Task AddOrUpdateRecurringJobsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<JobContext>> GetEnqueuedJobsAsync(string? queueName, CancellationToken cancellationToken);
    Task<IEnumerable<CronJob>> GetRecurringJobsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<CronJob>> GetRecurringJobsAsync(ISet<string> ids, CancellationToken cancellationToken);
    Task RemoveAsync(ISet<string> identifiants, CancellationToken cancellationToken);
    Task RemoveAsync(string? identifiant, CancellationToken cancellationToken);
    Task RemoveAllAsync(CancellationToken cancellationToken);
    Task TriggerAsync(CancellationToken cancellationToken);
    Task TriggerAsync(string? identifiant, CancellationToken cancellationToken);
    Task<SystemStatistics> GetStatisticsAsync(CancellationToken cancellationToken);
}