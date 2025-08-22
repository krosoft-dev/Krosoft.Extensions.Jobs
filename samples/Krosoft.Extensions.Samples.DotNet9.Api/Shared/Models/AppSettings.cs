namespace Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

internal record AppSettings
{
    public IEnumerable<JobAmqpSettings> JobsAmqp { get; set; } = new List<JobAmqpSettings>();
}