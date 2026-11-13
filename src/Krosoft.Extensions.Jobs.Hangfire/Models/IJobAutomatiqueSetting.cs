using Hangfire;

namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public interface IJobAutomatiqueSetting
{
    public string? Identifiant { get; set; }
    public string? CronExpression { get; set; }
    public string? Type { get; set; }
    public string? QueueName { get; set; }

    /// <summary>
    /// Mode de rattrapage des exécutions manquées. <c>null</c> applique <see cref="MisfireHandlingMode.Relaxed" />.
    /// </summary>
    public MisfireHandlingMode? MisfireHandling => null;
}
