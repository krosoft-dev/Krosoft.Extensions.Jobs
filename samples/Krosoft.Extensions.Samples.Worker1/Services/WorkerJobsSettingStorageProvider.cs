using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;

namespace Krosoft.Extensions.Samples.Worker1.Services;

internal class WorkerJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        jobsSettings.Add(AddJob(JobTypeCode.Worker1, "* * * * *"));
        jobsSettings.Add(AddJob(JobTypeCode.Shared, "* * * * *"));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *", string.Empty));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *", string.Empty));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *", string.Empty));
        jobsSettings.Add(AddSystemJob(JobTypeCode.SoLong, "* * * * *", "A"));

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
                                                       string cronExpression,
                                                       string suffix) =>
        new JobAutomatiqueSetting
        {
            Identifiant = $"System_{jobTypeCode}_{suffix}",
            CronExpression = cronExpression,
            Type = jobTypeCode.ToString(),
            QueueName = Constants.QueuesKeys.System
        };
}