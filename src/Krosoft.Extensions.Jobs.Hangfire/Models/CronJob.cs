namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public record CronJob
{
    public string? Identifiant { get; set; }

    public string? CronExpression { get; set; }

    public string? QueueName { get; set; }

    public DateTime? ProchaineExecutionDate { get; set; }

    public string? DerniereExecutionStatut { get; set; }

    public DateTime? DerniereExecutionDate { get; set; }

    public DateTime? CreationDate { get; set; }
}