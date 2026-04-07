using Krosoft.Extensions.Jobs.Hangfire.Interfaces;
using Krosoft.Extensions.Jobs.Hangfire.Models;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Shared.Services;

internal class SettingsJobsSettingStorageProvider : IJobsSettingStorageProvider
{
    public Task<IEnumerable<IJobAutomatiqueSetting>> GetAsync(CancellationToken cancellationToken)
    {
        var jobsSettings = new List<IJobAutomatiqueSetting>();
        jobsSettings.Add(AddWorkerJob(JobTypeCode.Mqtt, "* * * * *"));

        return Task.FromResult<IEnumerable<IJobAutomatiqueSetting>>(jobsSettings);
    }

    private static IJobAutomatiqueSetting AddWorkerJob(JobTypeCode jobTypeCode,
                                                       string cronExpression) =>
        new JobAutomatiqueSetting
        {
            Identifiant = $"Worker_{jobTypeCode}",
            CronExpression = cronExpression,
            Type = jobTypeCode.ToString(),
            QueueName = Constantes.QueuesKeys.Worker
        };
}