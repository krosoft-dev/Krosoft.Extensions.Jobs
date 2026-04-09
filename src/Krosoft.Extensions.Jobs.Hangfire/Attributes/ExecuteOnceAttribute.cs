using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Krosoft.Extensions.Jobs.Hangfire.Extensions;

namespace Krosoft.Extensions.Jobs.Hangfire.Attributes;

public class ExecuteOnceAttribute : JobFilterAttribute, IClientFilter, IServerFilter, IApplyStateFilter
{
    public void OnCreating(CreatingContext filterContext)
    {
        using (filterContext.Connection.AcquireDistributedLock(filterContext.Job.GetFingerprintLockKey(), TimeSpan.FromSeconds(15)))
        {
            var entries = filterContext.Connection.GetAllEntriesFromHash(filterContext.Job.GetFingerprintKey());
            if (entries != null && entries.ContainsKey("jobId"))
            {
                filterContext.SetJobParameter("Reason", "Job already queued.");
                filterContext.Canceled = true;
                return;
            }

            using var transaction = JobStorage.Current.GetConnection().CreateWriteTransaction();
            transaction.SetRangeInHash(filterContext.Job.GetFingerprintKey(), [new KeyValuePair<string, string>("jobId", "pending")]);
            transaction.Commit();
        }
    }

    public void OnCreated(CreatedContext filterContext)
    {
    }

    public void OnPerforming(PerformingContext context)
    {
        Console.WriteLine($"[ExecuteOnce] OnPerforming - Job: {context.BackgroundJob.Id}");
        Console.WriteLine($"[ExecuteOnce] LockKey: {context.BackgroundJob.Job.GetFingerprintLockKey()}:executing");

        try
        {
            var distributedLock = context.Connection.AcquireDistributedLock(
                                                                            $"{context.BackgroundJob.Job.GetFingerprintLockKey()}:executing",
                                                                            TimeSpan.FromSeconds(1));
            Console.WriteLine($"[ExecuteOnce] Lock acquired for job: {context.BackgroundJob.Id}");
            context.Items["DistributedLock"] = distributedLock;
        }
        catch (DistributedLockTimeoutException ex)
        {
            Console.WriteLine($"[ExecuteOnce] Lock failed for job: {context.BackgroundJob.Id} - {ex.Message}");
            context.SetJobParameter("Reason", "Job is already running on another server, execution cancelled.");
            context.Canceled = true;
        }
    }

    public void OnPerformed(PerformedContext context)
    {
        Console.WriteLine($"[ExecuteOnce] OnPerformed - Job: {context.BackgroundJob.Id}, Canceled: {context.Canceled}");
    
        if (context.Items.TryGetValue("DistributedLock", out var lockObj) && lockObj is IDisposable distributedLock)
        {
            Console.WriteLine($"[ExecuteOnce] Releasing lock for job: {context.BackgroundJob.Id}");
            distributedLock.Dispose();
        }
        else
        {
            Console.WriteLine($"[ExecuteOnce] No lock found in Items for job: {context.BackgroundJob.Id}");
        }

        using var transaction = context.Connection.CreateWriteTransaction();
        transaction.RemoveHash(context.BackgroundJob.Job.GetFingerprintKey());
        transaction.Commit();
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is DeletedState)
        {
            transaction.RemoveHash(context.BackgroundJob.Job.GetFingerprintKey());
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }
}