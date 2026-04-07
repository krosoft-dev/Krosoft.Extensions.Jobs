//using System.Security.Cryptography;
//using System.Text;
//using Hangfire.Client;
//using Hangfire.Common;
//using Hangfire.Server;

//namespace Krosoft.Extensions.Jobs.Hangfire.Attributes;

//public class ExecuteOnceAttribute : JobFilterAttribute, IClientFilter, IServerFilter
//{
//    public void OnCreating(CreatingContext filterContext)
//    {
//        var entries = filterContext.Connection.GetAllEntriesFromHash(GetJobKey(filterContext.Job));
//        if (entries != null && entries.ContainsKey("jobId"))
//        {
//            filterContext.SetJobParameter("Reason", "This job was already created once, cancel creation.");
//            filterContext.Canceled = true;
//        }
//    }

//    public void OnCreated(CreatedContext filterContext)
//    {
//        if (!filterContext.Canceled)
//        {
//            // Job created, mark it as such.
//            filterContext.Connection.SetRangeInHash(GetJobKey(filterContext.Job), [new KeyValuePair<string, string>("jobId", filterContext.BackgroundJob.Id)]);
//        }
//    }

//    public void OnPerforming(PerformingContext context)
//    {
//        var entries = context.Connection.GetAllEntriesFromHash(GetJobKey(context.BackgroundJob.Job));
//        if (entries != null && entries.ContainsKey("jobId"))
//        {
//            context.SetJobParameter("Reason", "This job was already created once, cancel creation.");
//            context.Canceled = true;
//        }
//    }

//    public void OnPerformed(PerformedContext context)
//    {
//        if (!context.Canceled)
//        {
//            // Job created, mark it as such.
//            context.Connection.SetRangeInHash(GetJobKey(context.BackgroundJob.Job), [new KeyValuePair<string, string>("jobId", context.BackgroundJob.Id)]);
//        }
//    }

//    private static string GetJobKey(Job? job)
//    {
//        if (job == null) return string.Empty;
//        //using var sha512 = SHA512.Create();
//        //return "execute-once:" + Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(job.ToString())));

//        return job.GetFingerprintKey();
//    }
//}

//internal static class JobExtensions
//{
//    //public static bool SkipConcurrentExecution(this Job job)
//    //    => job.Method.GetCustomAttributes(typeof(SkipConcurrentExecutionAttribute), false).Length > 0;

//    public static string GetFingerprintLockKey(this Job job) => $"{job.GetFingerprintKey()}:lock";
//    public static string GetFingerprintKey(this Job job) => $"fingerprint:{job.GetFingerprint()}";

//    private static string GetFingerprint(this Job job)
//    {
//        if (job.Type == null || job.Method == null) { return string.Empty; }
//        var parameters = string.Empty;

//        if (job.Args is not null) { parameters = string.Join(".", job.Args); }

//        return $"{job.Type.FullName}.{job.Method.Name}.{parameters}";
//    }
//}