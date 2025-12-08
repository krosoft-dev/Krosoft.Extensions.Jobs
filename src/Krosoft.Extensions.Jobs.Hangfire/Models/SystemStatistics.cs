namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public record SystemStatistics
{
    public long Enqueued { get; set; }
    public long Processing { get; set; }
    public long Succeeded { get; set; }
    public long Failed { get; set; }
    public IEnumerable<SystemServer> Servers { get; set; } = new List<SystemServer>();
}