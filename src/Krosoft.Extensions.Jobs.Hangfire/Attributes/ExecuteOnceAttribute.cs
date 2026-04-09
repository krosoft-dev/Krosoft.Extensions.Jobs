using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;

namespace Krosoft.Extensions.Jobs.Hangfire.Attributes;

public class ExecuteOnceAttribute : JobFilterAttribute, IClientFilter, IServerFilter
{
    public void OnCreating(CreatingContext filterContext)
    {
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var isAlreadyQueued = monitoringApi.IsJobAlreadyQueued(filterContext.Job);
        var isAlreadyProcessing = monitoringApi.IsJobAlreadyProcessing(filterContext.Job, null);

        if (isAlreadyQueued || isAlreadyProcessing)
        {
            filterContext.SetJobParameter("Reason", isAlreadyProcessing ? "Job already processing." : "Job already queued.");
            filterContext.Canceled = true;
        }
    }

    public void OnCreated(CreatedContext filterContext)
    {
    }

    public void OnPerforming(PerformingContext context)
    {
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var isAlreadyProcessing = monitoringApi.IsJobAlreadyProcessing(context.BackgroundJob.Job, context.BackgroundJob.Id);
        if (isAlreadyProcessing)
        {
            context.SetJobParameter("Reason", "Job is already running on another server, execution cancelled.");
            context.Canceled = true;
        }
    }

    public void OnPerformed(PerformedContext context)
    {
    }
}