using Hangfire;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;
using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;
using StackExchange.Redis;

namespace Krosoft.Extensions.Samples.Worker1.Services;

internal class WorkerJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        jobsSettings.Add(AddJob(JobTypeCode.Worker1, "* * * * *"));
        jobsSettings.Add(AddJob(JobTypeCode.Shared, "* * * * *"));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *"));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *"));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *"));

        return Task.FromResult<IEnumerable<IJobAutomatiqueSetting>>(jobsSettings);
    }

    private static IJobAutomatiqueSetting AddJob(JobTypeCode jobTypeCode,
                                                 string cronExpression) =>
        new JobAutomatiqueSetting
        {
            Identifiant = $"Job_{jobTypeCode}",
            CronExpression = cronExpression,
            Type = jobTypeCode.ToString(),
            QueueName = Constants.QueuesKeys.Worker1
        };

    private static IJobAutomatiqueSetting AddSystemJob(JobTypeCode jobTypeCode,
                                                       string cronExpression) =>
        new JobAutomatiqueSetting
        {
            Identifiant = $"System_{jobTypeCode}",
            CronExpression = cronExpression,
            Type = jobTypeCode.ToString(),
            QueueName = Constants.QueuesKeys.System
        };
}







public class ExecuteOnceCleanupService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var processingJobs = monitoringApi.ProcessingJobs(0, int.MaxValue);
        
        using var connection = JobStorage.Current.GetConnection();
        foreach (var job in processingJobs)
        {
            var key = job.Value.Job?.GetFingerprintLockKey();
            if (key != null)
            {
                using var transaction = connection.CreateWriteTransaction();
                transaction.RemoveHash(key);
                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}