namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

internal record JobAmqpSettings
{
    public string Identifiant { get; set; } = null!;
    public string CronExpression { get; set; } = null!;
    public string QueueName { get; set; } = null!;
}