using System.Security.Cryptography;
using System.Text;
using Hangfire.Client;
using Hangfire.Common;

namespace Krosoft.Extensions.Jobs.Hangfire.Attributes;

public class ExecuteOnceAttribute : JobFilterAttribute, IClientFilter
{
    public void OnCreating(CreatingContext filterContext)
    {
        var entries = filterContext.Connection.GetAllEntriesFromHash(GetJobKey(filterContext.Job));
        if (entries != null && entries.ContainsKey("jobId"))
        {
            // This job was already created once, cancel creation.
            filterContext.Canceled = true;
        }
    }

    public void OnCreated(CreatedContext filterContext)
    {
        if (!filterContext.Canceled)
        {
            // Job created, mark it as such.
            filterContext.Connection.SetRangeInHash(GetJobKey(filterContext.Job), [new KeyValuePair<string, string>("jobId", filterContext.BackgroundJob.Id)]);
        }
    }

    private static string GetJobKey(Job job)
    {
        using var sha512 = SHA512.Create();
        return "execute-once:" + Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(job.ToString())));
    }
}