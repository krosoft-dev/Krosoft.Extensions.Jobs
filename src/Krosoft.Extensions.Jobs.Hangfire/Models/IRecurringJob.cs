namespace Krosoft.Extensions.Jobs.Hangfire.Models;

public interface IRecurringJob
{
    string Type { get; }

    /// <summary>
    /// Démarre un job récurrent de manière asynchrone.
    /// </summary>
    /// <param name="identifiant">Identifiant du job récurrent.</param>
    /// <param name="cancellationToken">
    /// Token d'annulation fourni par Hangfire : il se déclenche à l'arrêt du serveur
    /// ou à l'abandon du job, ce qui permet d'interrompre les traitements longs.
    /// </param>
    /// <returns>Résultat de l'exécution.</returns>
    Task<JobResult> ExecuteAsync(string identifiant,
                                 CancellationToken cancellationToken);
}
