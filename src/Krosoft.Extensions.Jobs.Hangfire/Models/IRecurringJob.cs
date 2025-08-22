namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public interface IRecurringJob
{
    string Type { get; }

    Task<JobResult> ExecuteAsync(string identifiant);
}