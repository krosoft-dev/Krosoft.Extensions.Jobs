namespace Krosoft.Extensions.Jobs.Tests.Core;

internal record TriggerCreateCommand
{
    public string? FlowId { get; set; }

    public string? CronExpression { get; set; }
}