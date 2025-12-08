namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public interface IJob
{
    string Type { get; }

    /// <summary>
    /// Démarre un job de manière asynchrone.
    /// </summary>
    /// <param name="jobContext">Contexte du job.</param>
    /// <param name="cancellationToken">Token d'annulation.</param>
    /// <returns>Tache asynchrone.</returns>
    Task ExecuteAsync(JobContext jobContext,
                      CancellationToken cancellationToken);
}


 
internal record SystemStatistics
{

    public long Enqueued { get; set; }
    public long Processing { get; set; }
    public long Succeeded { get; set; }
    public long Failed { get; set; }
    public IEnumerable<SystemServer> Servers { get; set; } = new List<SystemServer>();
}

 

internal record SystemServer
{
    public string? Name { get; set; }
    public int WorkersCount { get; set; }
    public IList<string> Queues { get; set; } = new List<string>();
    public DateTime StartedAt { get; set; }
    public DateTime? Heartbeat { get; set; }
}