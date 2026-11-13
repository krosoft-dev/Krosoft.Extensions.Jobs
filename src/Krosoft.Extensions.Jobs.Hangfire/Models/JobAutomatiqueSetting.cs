using Krosoft.Extensions.Jobs.Models;

namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public record JobAutomatiqueSetting : IJobAutomatiqueSetting
{
    public string? Identifiant { get; set; }
    public string? CronExpression { get; set; }
    public string? Type { get; set; }
    public string? QueueName { get; set; }

    /// <summary>
    /// Politique de rattrapage des exécutions manquées. <c>null</c> applique <see cref="MissedExecutionPolicy.CatchUpOnce" />.
    /// </summary>
    public MissedExecutionPolicy? MissedExecution { get; set; }
}
