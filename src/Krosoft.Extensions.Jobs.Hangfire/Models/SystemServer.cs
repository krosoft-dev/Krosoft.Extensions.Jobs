namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public record SystemServer
{
    public string? Name { get; set; }
    public int WorkersCount { get; set; }
    public IList<string> Queues { get; set; } = new List<string>();
    public DateTime StartedAt { get; set; }
    public DateTime? Heartbeat { get; set; }
}