using Hangfire.Common;
using Hangfire.Storage;

namespace Krosoft.Extensions.Jobs.Hangfire.Extensions;

public static class MonitoringApiExtensions
{
    public static bool IsJobAlreadyProcessing(this IMonitoringApi monitoringApi, Job job, string? excludeJobId)
    {
        var fingerprint = job.GetFingerprintKey();
        var processingJobs = monitoringApi.ProcessingJobs(0, int.MaxValue);
        return processingJobs.Any(j => (excludeJobId == null || j.Key != excludeJobId) && j.Value?.Job?.GetFingerprintKey() == fingerprint);
    }

    public static bool IsJobAlreadyQueued(this IMonitoringApi monitoringApi, Job job)
    {
        var fingerprint = job.GetFingerprintKey();

        foreach (var queue in monitoringApi.Queues())
        {
            var enqueuedJobs = monitoringApi.EnqueuedJobs(queue.Name, 0, int.MaxValue);
            if (enqueuedJobs.Any(j => j.Value?.Job?.GetFingerprintKey() == fingerprint))
            {
                return true;
            }
        }

        return false;
    }
}
