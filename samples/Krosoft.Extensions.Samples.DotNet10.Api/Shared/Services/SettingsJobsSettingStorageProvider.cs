using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Shared.Services;

internal class SettingsJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        jobsSettings.Add(AddJob(JobTypeCode.Mqtt, "* * * * *"));

        return Task.FromResult<IEnumerable<IJobAutomatiqueSetting>>(jobsSettings);
    }

    private static IJobAutomatiqueSetting AddJob(JobTypeCode jobTypeCode,
                                                 string cronExpression) =>
        new JobAutomatiqueSetting
        {
            Identifiant = $"Job_{jobTypeCode}",
            CronExpression = cronExpression,
            Type = jobTypeCode.ToString(),
            QueueName = Constants.QueuesKeys.Api10
        };
}