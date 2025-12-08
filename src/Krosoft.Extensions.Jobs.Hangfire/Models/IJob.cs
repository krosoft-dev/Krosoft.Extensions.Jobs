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