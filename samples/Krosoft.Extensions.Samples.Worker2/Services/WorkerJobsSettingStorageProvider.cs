using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;

namespace Krosoft.Extensions.Samples.Worker2.Services;

internal class WorkerJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        jobsSettings.Add(AddJob(JobTypeCode.Worker2, "* * * * *"));
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
            QueueName = Constants.QueuesKeys.Worker2
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