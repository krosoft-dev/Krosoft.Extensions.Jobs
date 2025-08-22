using Krosoft.Extensions.Samples.DotNet9.Api.Shared.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Jobs.Jobs;

public record JobDto
{
    public string? Identifiant { get; set; }
    public string? CronExpression { get; set; }
    public JobTypeCode TypeCode { get; set; }
    public DateTime? ProchaineExecutionDate { get; set; }
    public string? DerniereExecutionStatut { get; set; }
    public DateTime? DerniereExecutionDate { get; set; }
    public DateTime? CreationDate { get; set; }
}